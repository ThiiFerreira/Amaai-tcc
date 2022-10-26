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
            var codigoRetorno = Request.Query["hub.challenge"];
            return codigoRetorno;
        }

        [HttpPost("whatsapp")]
        public IActionResult PostEvent([FromBody] JsonElement json)
        {
            //imprime a notificação
            _lastEvent = json;
            //_logger.LogInformation($"{nameof(PostEvent)} | Notificação recebida: " +
            //    JsonSerializer.Serialize(json,
            //        options: new() { WriteIndented = true }));

            var obj = JsonDocument.Parse(json.ToString());
            var telefone = "";
           

            // bloco que captura se o botao finalizar tarefa foi apertado
            if (json.ToString().Contains("button_reply"))
            {
                try
                {
                    var mensagem = obj.RootElement.GetProperty("entry")[0]
                       .GetProperty("changes")[0]
                       .GetProperty("value")
                       .GetProperty("messages")[0]
                       .GetProperty("interactive")
                       .GetProperty("button_reply")
                       .GetProperty("title")
                       .ToString();

                    var subMensagem = mensagem.Substring(0, 9);

                    if (subMensagem == "Finalizar")
                    {
                        var codigoStr = mensagem.Substring(17, 3);
                        var codigo = int.Parse(codigoStr);
                        _tarefaService.AtualizaTarefaParaFinalizada(codigo);

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if (json.ToString().Contains("contacts")!)
            {
                //implementar solução depois
            }
            else
            {
                try
                {
                    telefone = obj.RootElement.GetProperty("entry")[0]
                            .GetProperty("changes")[0]
                            .GetProperty("value")
                            .GetProperty("contacts")[0]
                            .GetProperty("wa_id")
                            .ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Falha ao identificar telefone");
                }
                if (telefone!="")
                    _mensagemWpp.enviaMensagemDeErro(telefone);
            }

            return Ok();
        }
    }
}