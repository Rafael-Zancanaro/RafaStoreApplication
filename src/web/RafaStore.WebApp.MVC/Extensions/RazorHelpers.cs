using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.Razor;

namespace RafaStore.WebApp.MVC.Extensions;

public static class RazorHelpers
{
    public static string HashEmailForGravatar(this RazorPage page, string email)
    {
        var data = MD5.HashData(Encoding.UTF8.GetBytes(email));
        var sBuilder = new StringBuilder();
        foreach (var t in data)
        {
            sBuilder.Append(t.ToString("x2"));
        }

        return sBuilder.ToString();
    }

    public static string MensagemEstoque(this RazorPage page, int quantidade)
    {
        return quantidade > 0 ? $"Apenas {quantidade} em estoque!" : "Produto esgotado!";
    }

    public static string FormatoMoeda(this RazorPage page, decimal valor)
    {
        return valor > 0 ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", valor) : "Gratuito";
    }
}