using APP.Models;
using AutoMapper;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Data;
using UsuariosApi.Data.Dtos.Tarefa;
using UsuariosApi.Helpers;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class TarefaService
    {
        private UserDbContext _context;
        private IMapper _mapper;
        
        public TarefaService(UserDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ReadTarefaDto AdicionaTarefa(CreateTarefaDto createTarefaDto, int usuarioId)
        {
            var _assistido = _context.UsuarioAssistido.FirstOrDefault(assistido => assistido.ResponsavelId == usuarioId);
            var tarefa = _mapper.Map<Tarefa>(createTarefaDto);
            tarefa.ResponsavelId = usuarioId;
            tarefa.IdosoId = _assistido.Id;
            tarefa.DataCriacao = DateTime.Now.ToString();
            _context.Tarefa.Add(tarefa);
            _context.SaveChanges();
            return _mapper.Map<ReadTarefaDto>(tarefa);
        }

        public ReadTarefaDto RecuperaTarefaPorId(int id, int usuarioId)
        {
            Tarefa tarefa = _context.Tarefa.FirstOrDefault(tarefa => tarefa.Id == id && (tarefa.IdosoId == usuarioId || tarefa.ResponsavelId == usuarioId));
            if (tarefa != null)
            {
                return _mapper.Map<ReadTarefaDto>(tarefa);
            }
            return null;
        }

        public List<ReadTarefaDto> RecuperaTarefa(int usuarioId)
        {
            List<Tarefa> list = _context.Tarefa.Where(tarefa => (tarefa.IdosoId == usuarioId || tarefa.ResponsavelId == usuarioId)).ToList();

            if (list != null)
            {
                return _mapper.Map<List<ReadTarefaDto>>(list);
            }
            return null;
        }

        public Result AtualizaTarefa(int id, CreateTarefaDto createTarefaDto, int usuarioId)
        {
            Tarefa tarefa = _context.Tarefa.FirstOrDefault(tarefa => tarefa.Id == id && (tarefa.IdosoId == usuarioId || tarefa.ResponsavelId == usuarioId));
            if (tarefa == null)
            {
                return Result.Fail("Tarefa não encontrada");
            }

            _mapper.Map(createTarefaDto, tarefa);
            _context.SaveChanges();
            return Result.Ok();

        }

        public Result DeletaTarefa(int id, int usuarioId)
        {
            Tarefa tarefa = _context.Tarefa.FirstOrDefault(tarefa => tarefa.Id == id &&  tarefa.ResponsavelId == usuarioId);
            if (tarefa == null)
            {
                return Result.Fail("Tarefa não encontrado");
            }

            _context.Tarefa.Remove(tarefa);
            _context.SaveChanges();
            return Result.Ok();
        }
    }
}
