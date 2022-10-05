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
            var fazerGet = true;

            if (fazerGet)
            {
                fazerGet = false;
                int milliseconds = 15000000;
                Thread.Sleep(milliseconds);
                Console.WriteLine("Oi sou eu de novo");
                WebRequest request = WebRequest.Create("https://localhost:6001/naodeixadormir");
                request.Method = "GET";
                var response = (HttpWebResponse)request.GetResponse();
            }
        }
    }
}
