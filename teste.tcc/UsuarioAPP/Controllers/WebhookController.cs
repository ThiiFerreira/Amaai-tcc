using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Models;

namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public void fazRequisicao()
        {
            Console.WriteLine("Oie");
        }
    }
}
