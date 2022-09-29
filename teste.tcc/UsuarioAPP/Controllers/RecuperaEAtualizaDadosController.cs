using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Data.Dtos.Usuario;
using UsuariosApi.Data.Dtos.UsuarioAssistido;
using UsuariosApi.Services;

namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class RecuperaEAtualizaDadosController : ControllerBase
    {
        private RecuperaEAtualizaDadosServices _service;

        public RecuperaEAtualizaDadosController(RecuperaEAtualizaDadosServices service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "responsavel")]

        public IActionResult RecuperaDadosUsuarioPorId(int id)
        {
            var usuario = _service.RecuperaDadosUsuarioPorId(id);
            if (usuario == null) return NotFound("Falha ao carregar dados");
            return Ok(usuario);
        }

        [HttpGet("assistido/{id}")]
        [Authorize(Roles = "responsavel")]
        public IActionResult RecuperaDadosUsuarioAssistidoPorId(int id)
        {
            var usuario = _service.RecuperaDadosUsuarioAssistidoPorId(id);
            if (usuario == null) return NotFound("Falha ao carregar dados");
            return Ok(usuario);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "responsavel")]

        public IActionResult AtualizaDadosUsuario([FromBody] UpdateUsuarioDto usuario,int id)
        { 
            var resultado = _service.AtualizaDadosUsuario(usuario,id);
            if (resultado.IsFailed) return NotFound("Falha ao carregar dados");
            return NoContent();
        }

        [HttpPut("assistido/{id}")]
        [Authorize(Roles = "responsavel")]
        public IActionResult AtualizaDadosUsuarioAssistido([FromBody] UpdateUsuarioAssitidoDto usuario, int id)
        {
            var resultado = _service.AtualizaDadosUsuarioAssistido(usuario, id);
            if (resultado.IsFailed) return NotFound("Falha ao carregar dados");
            return NoContent();
        }

        [HttpGet("assistido/{id}/contato")]
        [Authorize(Roles = "responsavel")]
        public IActionResult RecuperaContatoUsuarioAssistidoPorId(int id)
        {
            var telefoneUsuario = _service.RecuperaContatoUsuarioAssistidoPorIdDoSeuResponsavel(id);
            if (telefoneUsuario == null) return NotFound("Falha ao carregar contato");
            return Ok(telefoneUsuario);
        }


    }
}
