using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RafaStore.Identidade.API.Models;
using RafaStore.WebAPI.Core.Identidade;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RafaStore.Core.Messages.Integration;
using RafaStore.MessageBus;
using RafaStore.WebAPI.Core.Controllers;

namespace RafaStore.Identidade.API.Controllers;

[Route("api/identidade")]
public class AuthController(SignInManager<IdentityUser> signInManager,
                            UserManager<IdentityUser> userManager,
                            IOptions<AppSettings> appSettings,
                            IMessageBus bus) : MainController
{
    [HttpPost("nova-conta")]
    public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var user = new IdentityUser
        {
            UserName = usuarioRegistro.Email,
            Email = usuarioRegistro.Email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, usuarioRegistro.Senha);
        if (result.Succeeded)
        {
            var clienteResult = await RegistrarCliente(usuarioRegistro);
            if (clienteResult.ValidationResult.IsValid) 
                return CustomResponse(await GerarJwt(usuarioRegistro.Email));
            
            await userManager.DeleteAsync(user);
                
            return CustomResponse(clienteResult.ValidationResult);
        }

        foreach (var error in result.Errors)
            AdicionarErroProcessamento(error.Description);

        return CustomResponse();
    }

    [HttpPost("autenticar")]
    public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var result = await signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);
        if (result.Succeeded)
            return CustomResponse(await GerarJwt(usuarioLogin.Email));

        AdicionarErroProcessamento(result.IsLockedOut
            ? ErrosResources.UsuarioBloqueado
            : ErrosResources.UsuarioSenhaIncorreto);

        return CustomResponse();
    }

    #region Metodos Privados

    private async Task<UsuarioRespostaLogin> GerarJwt(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        var claims = await userManager.GetClaimsAsync(user);

        var identityClaims = await ObterClaimUsuario(claims, user);
        var encodedToken = CodificarToken(identityClaims);

        return ObterRespostaToken(encodedToken, user, claims);
    }

    private async Task<ClaimsIdentity> ObterClaimUsuario(ICollection<Claim> claims, IdentityUser user)
    {
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

        var userRoles = await userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
            claims.Add(new Claim("role", userRole));

        return new ClaimsIdentity(claims);
    }

    private string CodificarToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(appSettings.Value.Secret);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = appSettings.Value.Emissor,
            Audience = appSettings.Value.ValidoEm,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(appSettings.Value.ExpiracaoHoras),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }

    private UsuarioRespostaLogin ObterRespostaToken(string encodedToken, IdentityUser user, ICollection<Claim> claims)
         => new()
         {
             AccessToken = encodedToken,
             ExpiresIn = TimeSpan.FromHours(appSettings.Value.ExpiracaoHoras).TotalSeconds,
             UsuarioToken = new UsuarioToken
             {
                 Id = user.Id,
                 Email = user.Email,
                 Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
             }
         };

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    
    
    private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
    {
        var usuario = await userManager.FindByEmailAsync(usuarioRegistro.Email);
        
        var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(Guid.Parse(usuario.Id), usuarioRegistro.Nome,
            usuarioRegistro.Email, usuarioRegistro.Cpf);

        try
        {
            return await bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
        }
        catch
        {
            await userManager.DeleteAsync(usuario);
            throw;
        }
    }
    
    #endregion
}