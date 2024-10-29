using Microsoft.Extensions.Options;
using RafaStore.WebApp.MVC.Extensions;
using RafaStore.WebApp.MVC.Models;

namespace RafaStore.WebApp.MVC.Services;

public class AutenticacaoService : Service, IAutenticacaoService
{
    private readonly HttpClient _httpClient;
    public AutenticacaoService(HttpClient client, IOptions<AppSettings> settings)
    {
        client.BaseAddress = new Uri(settings.Value.AutenticacaoUrl);

        _httpClient = client;
    }

    public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
    {
        var loginContent = ObterConteudo(usuarioLogin);

        var response = await _httpClient.PostAsync("/api/identidade/autenticar", loginContent);

        return TratarErrosResponse(response)
            ? await DeserializarObjetoResponse<UsuarioRespostaLogin>(response)
            : new UsuarioRespostaLogin(await DeserializarObjetoResponse<ResponseResult>(response));
    }

    public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
    {
        var registroContent = ObterConteudo(usuarioRegistro);

        var response = await _httpClient.PostAsync("/api/identidade/nova-conta", registroContent);

        return TratarErrosResponse(response)
            ? await DeserializarObjetoResponse<UsuarioRespostaLogin>(response)
            : new UsuarioRespostaLogin(await DeserializarObjetoResponse<ResponseResult>(response));
    }
}