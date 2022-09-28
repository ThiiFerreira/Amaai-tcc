using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Data.Dtos.Usuario;
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


    }
}
