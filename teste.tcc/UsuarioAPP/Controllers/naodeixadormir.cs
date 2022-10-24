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
        public IActionResult fazRequisicao()
        {
            Console.WriteLine("Criando cronometro para chamar loop");
            criaCronometro(60000);
            Console.WriteLine("Cronometro criado");

            return NoContent();
        }

        private void criaCronometro(double tempo)
        {
            var cronometro = new System.Timers.Timer();
            cronometro.Enabled = false;
            cronometro.Interval = tempo;
            cronometro.AutoReset = false;
            cronometro.Elapsed += async (sender, e) => realizaLoopInfinito();
            cronometro.Start();
        }

        private void realizaLoopInfinito()
        {
            int i = 1;
            while (i != 0)
            {
                
                Console.WriteLine($"Oi acordei pela {i}° vez");

                i = i + 1;
                int milliseconds = 900000;
                Thread.Sleep(milliseconds);
            }
        }
    }
}
