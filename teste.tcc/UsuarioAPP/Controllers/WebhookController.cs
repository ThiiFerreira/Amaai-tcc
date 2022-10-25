using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace WebAppMonitoramentoWebhook.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WebhookMonitorController : ControllerBase
    {
        private readonly ILogger<WebhookMonitorController> _logger;
        private static object? _lastEvent;

        public WebhookMonitorController(ILogger<WebhookMonitorController> logger)
        {
            _logger = logger;
            
        }

        [HttpGet]
        public int GetLastEvent([FromQuery] object data)
        {
            System.Console.WriteLine(data.GetHashCode());
            return data.GetHashCode();
        }

        [HttpPost]
        public IActionResult PostEvent(object data)
        {
            _lastEvent = data;
            _logger.LogInformation($"{nameof(PostEvent)} | Notifica��o recebida: " +
                JsonSerializer.Serialize(data,
                    options: new() { WriteIndented = true }));
            return Ok();
        }
    }
}