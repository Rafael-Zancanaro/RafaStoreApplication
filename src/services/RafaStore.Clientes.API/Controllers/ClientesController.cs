using Microsoft.AspNetCore.Mvc;
using RafaStore.Clientes.API.Application.Commands;
using RafaStore.Core.Mediator;
using RafaStore.WebAPI.Core.Controllers;

namespace RafaStore.Clientes.API.Controllers;

[ApiController]
public class ClientesController(IMediatorHandler mediatorHandler) : MainController
{
    [HttpGet("clientes")]
    public async Task<IActionResult> Index()
    {
        var resultado = await mediatorHandler.EnviarComando(new RegistrarClienteCommand(Guid.NewGuid(), "Rafael", "rafa10_zc@hotmail.com", "10012579955"));

        return CustomResponse(resultado);
    }
}