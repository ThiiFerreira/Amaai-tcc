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
            var teste = Request.Query["hub.challenge"];
            System.Console.WriteLine(teste);
            return teste;
        }

        [HttpPost("whatsapp")]
        public IActionResult PostEvent([FromBody] JsonElement json)
        {
            //imprime a notificação
            _lastEvent = json;
            _logger.LogInformation($"{nameof(PostEvent)} | Notificação recebida: " +
                JsonSerializer.Serialize(json,
                    options: new() { WriteIndented = true }));

            var obj = JsonDocument.Parse(json.ToString());

            var mensagem = "";
            var contato = "";

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

                contato = obj.RootElement.GetProperty("entry")[0]
                    .GetProperty("changes")[0]
                    .GetProperty("value")
                    .GetProperty("contacts")[0]
                    .GetProperty("wa_id")
                    .ToString();

                var subMensagem = mensagem.Substring(0, 9);

                if (subMensagem == "Finalizar")
                {
                    _mensagemWpp.enviarMensagemPedindoCodigoDaTarefa(contato);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //bloco que captura a mensagem com o codigo
            try
            {
                var codigoStr = obj.RootElement.GetProperty("entry")[0]
                    .GetProperty("changes")[0]
                    .GetProperty("value")
                    .GetProperty("messages")[0]
                    .GetProperty("text")
                    .GetProperty("body")
                    .ToString();
                var codigo = int.Parse(codigoStr);

                _tarefaService.AtualizaTarefaParaFinalizada(codigo);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return Ok();
        }
    }
}