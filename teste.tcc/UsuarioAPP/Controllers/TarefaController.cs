using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UsuariosApi.Data.Dtos.Tarefa;
using UsuariosApi.Helpers;
using UsuariosApi.Services;

namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class TarefaController : ControllerBase
    {
        private TarefaService _tarefaService;
        public TarefaController(TarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpPost]
        [Authorize(Roles = "responsavel")]
        public IActionResult AdicionaTarefa([FromBody] CreateTarefaDto createTarefaDto)
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);

            var resultado = _tarefaService.AdicionaTarefa(createTarefaDto, usuarioId);
            
            return Ok(resultado.Reasons);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "responsavel, idoso")]
        public IActionResult RecuperaTarefaPorId(int id)
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);
            //var tarefa = _tarefaServiceArray.RecuperaTarefaPorId(id);
            var tarefa = _tarefaService.RecuperaTarefaPorId(id, usuarioId);
            if (tarefa == null) return NotFound("Tarefa não encontrada");
            return Ok(tarefa);
        }

        [HttpGet]
        [Authorize(Roles = "responsavel, idoso")]
        public IActionResult RecuperaTarefa()
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);

            //List<ReadTarefaDto> listTarefa = _tarefaServiceArray.RecuperaTarefa();
            List<ReadTarefaDto> listTarefa = _tarefaService.RecuperaTarefa(usuarioId);
            if (listTarefa == null) return NotFound();
            return Ok(listTarefa);
        }

        [HttpGet("finalizadas")]
        [Authorize(Roles = "responsavel, idoso")]
        public IActionResult RecuperaTarefasFinalizadas()
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);

            //List<ReadTarefaDto> listTarefa = _tarefaServiceArray.RecuperaTarefa();
            List<ReadTarefaDto> listTarefa = _tarefaService.RecuperaTarefasFinalizadas(usuarioId);
            if (listTarefa == null) return NotFound();
            return Ok(listTarefa);
        }

        [HttpGet("excluidas")]
        [Authorize(Roles = "responsavel, idoso")]
        public IActionResult RecuperaTarefasExcluidas()
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);

            //List<ReadTarefaDto> listTarefa = _tarefaServiceArray.RecuperaTarefa();
            var listTarefa = _tarefaService.RecuperaTarefasExcluidas(usuarioId);
            if (listTarefa == null) return NotFound();
            return Ok(listTarefa);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "responsavel")]
        public IActionResult AtualizaTarefa(int id, [FromBody] CreateTarefaDto createTarefaDto)
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);

            Result resultado = _tarefaService.AtualizaTarefa(id, createTarefaDto, usuarioId);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/finalizar")]
        [Authorize(Roles = "responsavel, idoso")]
        public IActionResult AtualizaTarefaParaFinalizada(int id)
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);

            Result resultado = _tarefaService.AtualizaTarefaParaFinalizada(id);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "responsavel")]
        public IActionResult DeletaTarefa(int id)
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);

            Result resultado = _tarefaService.DeletaTarefa(id, usuarioId);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }
    }
}
