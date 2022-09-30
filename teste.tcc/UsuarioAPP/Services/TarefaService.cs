using APP.Models;
using AutoMapper;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
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

            tarefa.DataCriacao = DateTime.UtcNow.AddHours(-3).ToString("dd/MM/yyyy");
            
            tarefa.Titulo = tarefa.Titulo.ToUpper();
            tarefa.Descricao = tarefa.Descricao.ToUpper();
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
            List<Tarefa> list = _context.Tarefa.Where(tarefa => (tarefa.IdosoId == usuarioId || tarefa.ResponsavelId == usuarioId) && tarefa.Finalizada == false).ToList();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt");

            var listOrdenada = list.OrderBy(x => x.DataAlerta.ToString()).ThenBy(x => x.HoraAlerta);

            if (list != null)
            {
                foreach (var tarefa in list)
                {
                    DateTime data = DateTime.Parse("20/08/2022");
                    Console.WriteLine(data);

                }
                return _mapper.Map<List<ReadTarefaDto>>(listOrdenada);
            }
            return null;
        }
        public List<ReadTarefaDto> RecuperaTarefasFinalizadas(int usuarioId)
        {
            List<Tarefa> list = _context.Tarefa.Where(tarefa => (tarefa.IdosoId == usuarioId || tarefa.ResponsavelId == usuarioId) && tarefa.Finalizada == true).ToList();
            var listOrdenada = list.OrderByDescending(x => x.DataFinalizacao);

            if (list != null)
            {
                return _mapper.Map<List<ReadTarefaDto>>(listOrdenada);
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
            createTarefaDto.Titulo = createTarefaDto.Titulo.ToUpper();
            createTarefaDto.Descricao = createTarefaDto.Descricao.ToUpper();
            _mapper.Map(createTarefaDto, tarefa);
            _context.SaveChanges();
            return Result.Ok();

        }
        public Result AtualizaTarefaParaFinalizada(int id, int usuarioId)
        {
            Tarefa tarefa = _context.Tarefa.FirstOrDefault(tarefa => tarefa.Id == id && (tarefa.IdosoId == usuarioId || tarefa.ResponsavelId == usuarioId));
            if (tarefa == null)
            {
                return Result.Fail("Tarefa não encontrada");
            }
            tarefa.DataFinalizacao = DateTime.UtcNow.AddHours(-3).ToString("dd/MM/yyyy");
            tarefa.Finalizada = true;
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
