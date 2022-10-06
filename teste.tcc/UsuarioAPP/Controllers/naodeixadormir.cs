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
        static int i = 0;

        [HttpGet]
        public void fazRequisicao()
        {
            i=i+1;
            Console.WriteLine($"Oi acordei pela {i}° vez");
            int milliseconds = 60000;
            Thread.Sleep(milliseconds);
            //WebRequest request = WebRequest.Create("https://app-tcc-amai-producao.herokuapp.com/naodeixadormir");
            WebRequest request = WebRequest.Create("https://localhost:6001/naodeixadormir");
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();
        }
    }
}
