using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace WebAppMonitoramentoWebhook.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private static object? _lastEvent;

        public WebhookController(ILogger<WebhookController> logger)
        {
            _logger = logger;
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
            _lastEvent = json;
            _logger.LogInformation($"{nameof(PostEvent)} | Notificação recebida: " +
                JsonSerializer.Serialize(json,
                    options: new() { WriteIndented = true }));

            var obj = JsonDocument.Parse(json.ToString());   

            try
            {
                var mensagem = obj.RootElement.GetProperty("entry")[0]
                    .GetProperty("changes")[0]
                    .GetProperty("value")
                    .GetProperty("messages")[0]
                    .GetProperty("button")
                    .GetProperty("text")
                    .ToString();

                var subMensagem = mensagem.Substring(0, 9);

                if (subMensagem == "Finalizar")
                {
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Ok();
        }
    }
}