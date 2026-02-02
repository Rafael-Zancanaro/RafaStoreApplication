using RafaStore.WebApp.MVC.Extensions;
using RafaStore.WebApp.MVC.Models;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace RafaStore.WebApp.MVC.Services;

public abstract class Service
{
    private static JsonSerializerOptions JsonOptions => new() { PropertyNameCaseInsensitive = true };

    protected static StringContent ObterConteudo(object dado)
            => new(JsonSerializer.Serialize(dado),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);

    protected static async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        => JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), JsonOptions);

    protected static bool TratarErrosResponse(HttpResponseMessage response)
    {
        switch ((int)response.StatusCode)
        {
            case 401:
            case 403:
            case 404:
            case 500:
                throw new CustomHttpRequestException(response.StatusCode);

            case 400:
                return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }

    protected static ResponseResult RetornoOK()
    {
        return new ResponseResult();
    }
}