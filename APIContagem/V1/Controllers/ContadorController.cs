using Microsoft.AspNetCore.Mvc;
using APIContagem.V1.Models;

namespace APIContagem.V1.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("[controller]")]
[Route("v{version:apiVersion}/[controller]")]
public class ContadorController : ControllerBase
{
    private static readonly Contador _CONTADOR = new Contador();
    private readonly ILogger<ContadorController> _logger;
    private readonly IConfiguration _configuration;

    public ContadorController(ILogger<ContadorController> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet]
    public ResultadoContador GetV1_0()
    {
        int valorAtualContador;
        lock (_CONTADOR)
        {
            _CONTADOR.Incrementar();
            valorAtualContador = _CONTADOR.ValorAtual;
        }

        _logger.LogInformation($"Contador - Valor atual: {valorAtualContador}");

        var resultado = new ResultadoContador()
        {
            ValorAtual = valorAtualContador,
            Local = _CONTADOR.Local,
            Kernel = _CONTADOR.Kernel,
            Framework = _CONTADOR.Framework,
            Mensagem = _configuration["MensagemVariavel"]
        };

        return resultado;
    }
}