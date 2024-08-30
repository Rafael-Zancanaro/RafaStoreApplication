using RafaStore.WebApp.MVC.Models;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace RafaStore.WebApp.MVC.Services
{
    public class AutenticacaoService(HttpClient client) : IAutenticacaoService
    {
        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = new StringContent(
                JsonSerializer.Serialize(usuarioLogin),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            var response = await client.PostAsync("https://localhost:44312/api/identidade/autenticar", loginContent);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), jsonOptions);
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContent = new StringContent(
                JsonSerializer.Serialize(usuarioRegistro),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            var response = await client.PostAsync("https://localhost:44312/api/identidade/nova-conta", registroContent);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), jsonOptions);
        }
    }
}