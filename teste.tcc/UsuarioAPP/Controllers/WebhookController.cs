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
        public string GetLastEvent([FromQuery] object data)
        {

            var teste = Request.Query["hub.challenge"];
            System.Console.WriteLine(teste);
            return teste;
        }

        [HttpPost]
        public IActionResult PostEvent([FromQuery] object data)
        {
            var teste = Request.Body;
            System.Console.WriteLine(teste);
            return Ok();
        }
    }
}