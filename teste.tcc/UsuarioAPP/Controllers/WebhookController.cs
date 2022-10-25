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
    public class WebhookMonitorController : ControllerBase
    {

        [HttpGet]
        public string GetLastEvent([FromQuery] object data)
        {
            var teste = Request.Query["hub.challenge"];
            System.Console.WriteLine(teste);
            return teste;
        }

        [HttpPost]
        public IActionResult PostEvent([FromBody] JsonElement json)
        {
            var obj = JsonDocument.Parse(json.ToString());
            Console.WriteLine(json.ToString());
            var mensagem = obj.RootElement.GetProperty("value").GetProperty("messages")[0].GetProperty("text").GetProperty("body");
            Console.WriteLine(mensagem);

            return Ok();
        }
    }
}