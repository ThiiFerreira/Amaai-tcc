using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Data.Dtos.Usuario;
using UsuariosApi.Data.Dtos.UsuarioAssistido;
using UsuariosApi.Data.Requests;
using UsuariosApi.Helpers;
using UsuariosApi.Services;

namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class DadosController : ControllerBase
    {
        private RecuperaEAtualizaDadosServices _service;

        public DadosController(RecuperaEAtualizaDadosServices service)
        {
            _service = service;
        }

        [HttpGet("responsavel")]
        [Authorize(Roles = "responsavel")]

        public IActionResult RecuperaDadosUsuarioPorId()
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);
            var usuario = _service.RecuperaDadosUsuarioPorId(usuarioId);
            if (usuario == null) return NotFound("Falha ao carregar dados");
            return Ok(usuario);
        }

        [HttpGet("assistido")]
        [Authorize(Roles = "responsavel")]
        public IActionResult RecuperaDadosUsuarioAssistidoPorId()
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);
            var usuario = _service.RecuperaDadosUsuarioAssistidoPorIdDoResponsavel(usuarioId);
            if (usuario == null) return NotFound("Falha ao carregar dados");
            return Ok(usuario);
        }

        [HttpPut("responsavel")]
        [Authorize(Roles = "responsavel")]

        public IActionResult AtualizaDadosResponsavel([FromBody] UpdateUsuarioDto usuario)
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);
            var resultado = _service.AtualizaDadosResponsavel(usuario, usuarioId);
            if (resultado.IsFailed) return NotFound("Falha ao carregar dados");
            return NoContent();
        }

        [HttpPut("assistido")]
        [Authorize(Roles = "responsavel")]
        public IActionResult AtualizaDadosUsuarioAssistido([FromBody] UpdateUsuarioAssitidoDto usuario)
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);
            var resultado = _service.AtualizaDadosUsuarioAssistido(usuario, usuarioId);
            if (resultado.IsFailed) return NotFound("Falha ao carregar dados");
            return NoContent();
        }


        [HttpDelete("responsavel")]
        [Authorize(Roles = "responsavel")]
        public IActionResult ExcluirConta([FromBody] LoginRequest request)
        {
           var resultado = _service.ExcluirUsuario(request);
            if(resultado.IsFailed) return NotFound(resultado.Errors[0].Message);
            return NoContent();
        }
    }
}
