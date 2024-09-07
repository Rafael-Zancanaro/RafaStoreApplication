using Microsoft.AspNetCore.Mvc;
using RafaStore.WebApp.MVC.Models;

namespace RafaStore.WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult response)
        {
            if (response != null && response.Errors.Mensagens.Any())
            {
                return true;
            }

            return false;
        }
    }
}