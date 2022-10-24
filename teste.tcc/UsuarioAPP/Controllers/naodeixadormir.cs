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
        static int cont = 1;

        [HttpGet]
        public IActionResult fazRequisicao()
        {

            criaCronometro(600000);

            return NoContent();
        }

        [HttpGet("reset")]
        public IActionResult fazRequisicaoReset()
        {
            Console.WriteLine($"Oi fui chamado pela {cont}° vez");
            cont++;
            return NoContent();
        }

        private void criaCronometro(double tempo)
        {
            var cronometro = new System.Timers.Timer();
            cronometro.Enabled = false;
            cronometro.Interval = tempo;
            cronometro.Elapsed += async (sender, e) => realizaLoopInfinito();
            cronometro.Start();
        }

        private void realizaLoopInfinito()
        {
            
            WebRequest request = WebRequest.Create("https://app-tcc-amai-producao.herokuapp.com/naodeixadormir/reset");
            request.Method = "GET";
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.NoContent)
                {

                }
                else
                {
                    Console.WriteLine("Verificar rota nãodeixardormir/reset - falha detectada no else");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Verificar rota nãodeixardormir/reset - falha detectada no exceção");
                Console.WriteLine(e.Message);

            }
        }
    }
}
