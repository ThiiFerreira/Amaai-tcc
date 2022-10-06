using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class naodeixadormir : ControllerBase
    {

        [HttpGet]
        public void fazRequisicao()
        {
            int i = 1;
            while (i != 0)
            {
                
                Console.WriteLine($"Oi acordei pela {i}° vez");
                i = i + 1;
                int milliseconds = 1500000;
                Thread.Sleep(milliseconds);
            }
        }
    }
}
