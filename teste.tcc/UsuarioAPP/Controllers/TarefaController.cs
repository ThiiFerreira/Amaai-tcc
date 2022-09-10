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
        private TarefaServiceArray  _tarefaServiceArray;
        public TarefaController(TarefaService tarefaService, TarefaServiceArray tarefaServiceArray)
        {
            _tarefaService = tarefaService;
            _tarefaServiceArray = tarefaServiceArray;
        }

        [HttpPost]
        [Authorize(Roles = "responsavel")]
        public IActionResult AdicionaTarefa([FromBody] CreateTarefaDto createTarefaDto)
        {
            string token = Request.Headers["Authorization"];
            string subToken = token.Substring(7);
            var usuarioId = new HelpersUsuario().RetornarIdUsuario(subToken);

            ReadTarefaDto readTarefaDto = _tarefaService.AdicionaTarefa(createTarefaDto, usuarioId);
            return CreatedAtAction(nameof(RecuperaTarefaPorId), new { id = readTarefaDto.Id }, readTarefaDto);
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
