using APP.Data.Dtos.UsuarioAssistido;
using APP.Services;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CadastroAssistidoController : ControllerBase
    {
        private CadastroAssistidoService _assistidoService;
        public CadastroAssistidoController(CadastroAssistidoService assistidoService)
        {
            _assistidoService = assistidoService;
        }

        [HttpPost]
        [Authorize(Roles = "responsavel")]
        public IActionResult CadastraAssistido(CreateUsuarioAssistido createDto)
        {
            Result resultado = _assistidoService.CadastraAssistido(createDto);
            if (resultado.IsFailed) return StatusCode(500, resultado.Errors);
            return Ok(resultado.Successes);
        }
    }
}
