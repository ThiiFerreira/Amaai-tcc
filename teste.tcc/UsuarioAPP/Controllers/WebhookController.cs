using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using UsuariosApi.Services;

namespace WebAppMonitoramentoWebhook.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private static object? _lastEvent;
        private TarefaService _tarefaService;
        private MensagemWpp _mensagemWpp;


        public WebhookController(ILogger<WebhookController> logger, TarefaService tarefaService, MensagemWpp mensagemWpp)
        {
            _logger = logger;
            _tarefaService = tarefaService;
            _mensagemWpp = mensagemWpp;
        }

        [HttpGet]
        public object? GetLastEvent()
        {
            _logger.LogInformation($"{nameof(GetLastEvent)} | Último evento recebido: " +
                JsonSerializer.Serialize(_lastEvent,
                    options: new() { WriteIndented = true }));
            return _lastEvent;
        }

        [HttpGet("whatsapp")]
        public string GetLastEvent([FromQuery] object data)
        {
            var codigoDeRetorno = Request.Query["hub.challenge"];
            return codigoDeRetorno;
        }

        [HttpPost("whatsapp")]
        public IActionResult PostEvent([FromBody] JsonElement json)
        {
            imprime a notificação
            _lastEvent = json;
            _logger.LogInformation($"{nameof(PostEvent)} | Notificação recebida: " +
                JsonSerializer.Serialize(json,
                    options: new() { WriteIndented = true }));

            var obj = JsonDocument.Parse(json.ToString());
            var mensagem = "";
            var telefone = obj.RootElement.GetProperty("entry")[0]
                        .GetProperty("changes")[0]
                        .GetProperty("value")
                        .GetProperty("contacts")[0]
                        .GetProperty("wa_id")
                        .ToString();


            if (json.ToString().Contains("button"))
            {
                // bloco que captura se o botao finalizar tarefa foi apertado
                try
                {
                    mensagem = obj.RootElement.GetProperty("entry")[0]
                       .GetProperty("changes")[0]
                       .GetProperty("value")
                       .GetProperty("messages")[0]
                       .GetProperty("button")
                       .GetProperty("text")
                       .ToString();

                    var subMensagem = mensagem.Substring(0, 9);
                    Console.WriteLine(subMensagem);


                    if (subMensagem == "Finalizar")
                    {
                        
                        var codigoStr = mensagem.Substring(17,3);
                        var codigo = int.Parse(codigoStr);

                       // _mensagemWpp.enviarMensagemPedindoCodigoDaTarefa(telefone);

                        if (codigo is int)
                        {
                            _tarefaService.AtualizaTarefaParaFinalizada(codigo);
                        }
                        else
                        {
                            _mensagemWpp.enviaMensagemDeErro(telefone);

                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if (json.ToString().Contains("body"))
            {
                Console.WriteLine("dentro do body");
                try
                {
                    //bloco que captura a mensagem com o codigo
                    var codigoStr = obj.RootElement.GetProperty("entry")[0]
                        .GetProperty("changes")[0]
                        .GetProperty("value")
                        .GetProperty("messages")[0]
                        .GetProperty("text")
                        .GetProperty("body")
                        .ToString();
                    var codigo = int.Parse(codigoStr);

                    if (codigo is int)
                    {
                        _tarefaService.AtualizaTarefaParaFinalizada(codigo);
                    }
                    else
                    {
                        _mensagemWpp.enviaMensagemDeErro(telefone);

                    }

                }
                catch (Exception e)
                {
                    _mensagemWpp.enviaMensagemDeErro(telefone);

                }
            }
            else
            {
                //não foi possivel identificar a mensagem
                _mensagemWpp.enviaMensagemDeErro(telefone);
            }    

            return Ok();
        }
    }
}