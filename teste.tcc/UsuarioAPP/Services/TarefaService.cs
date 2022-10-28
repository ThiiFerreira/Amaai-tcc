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
        private MensagemWpp _mensagemWpp;


        public TarefaService(UserDbContext context, IMapper mapper, MensagemWpp mensagemWpp)
        {
            _context = context;
            _mapper = mapper;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt");
            _mensagemWpp = mensagemWpp;
        }

        public Result AdicionaTarefa(CreateTarefaDto createTarefaDto, int usuarioId)
        {
            var _assistido = _context.UsuarioAssistido.FirstOrDefault(assistido => assistido.ResponsavelId == usuarioId);
            var tarefa = _mapper.Map<Tarefa>(createTarefaDto);
            tarefa.ResponsavelId = usuarioId;
            tarefa.IdosoId = _assistido.Id;

            tarefa.DataCriacao = DateTime.Now.ToString("dd/MM/yyyy");

            tarefa.Titulo = tarefa.Titulo.ToUpper();
            tarefa.Descricao = tarefa.Descricao.ToUpper();
            _context.Tarefa.Add(tarefa);
            _context.SaveChanges();
            var tempo = retornaTimer(tarefa);

            if(tempo>0)
                criaCronometro(tempo,tarefa, _assistido.Telefone, _assistido.Nome);
            else
                Console.WriteLine("Cronometro para realizar tarefa não iniciado");

            var resultadoMensagem = _mensagemWpp.EnviarMensagemAlertaTarefa(tarefa, _assistido.Telefone);

            if (resultadoMensagem.IsFailed)
            {
                return Result.Ok().WithSuccess("Tarefa criada - Falha ao enviar mensagem para o whastapp");
            }

            return Result.Ok().WithSuccess("Tarefa criada - Mensagem enviada para o assistido");     
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

            var listOrdenada = list.OrderBy(x => DateTime.Parse(x.DataAlerta.ToString())).ThenBy(x => x.HoraAlerta);           

            if (listOrdenada != null)
            {
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

        public List<ReadTarefaExcluidaDto> RecuperaTarefasExcluidas(int usuarioId)
        {
            List<TarefaExcluida> list = _context.Tarefa_Excluida.Where(tarefa => (tarefa.IdosoId == usuarioId || tarefa.ResponsavelId == usuarioId)).ToList();
            var listOrdenada = list.OrderByDescending(x => x.DataExclusao);

            if (list != null)
            {
                return _mapper.Map<List<ReadTarefaExcluidaDto>>(listOrdenada);
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
        public Result AtualizaTarefaParaFinalizada(int id)
        {

            Tarefa tarefa = _context.Tarefa.FirstOrDefault(tarefa => tarefa.Id == id);
            if (tarefa == null)
            {
                return Result.Fail("Tarefa não encontrada");
            }
            var _assistido = _context.UsuarioAssistido.FirstOrDefault(assistido => assistido.Id == tarefa.IdosoId);

            tarefa.DataFinalizacao = DateTime.UtcNow.AddHours(-3).ToString("dd/MM/yyyy");
            tarefa.DataFinalizacao = DateTime.Now.ToString("dd/MM/yyyy");
            tarefa.Finalizada = true;
            _context.SaveChanges();

            _mensagemWpp.enviaFeedbackTarefaFinalizada(_assistido.Nome, tarefa, _assistido.Telefone);
            return Result.Ok();

        }

        public Result DeletaTarefa(int id, int usuarioId)
        {
            Tarefa tarefa = _context.Tarefa.FirstOrDefault(tarefa => tarefa.Id == id &&  tarefa.ResponsavelId == usuarioId);
            if (tarefa == null)
            {
                return Result.Fail("Tarefa não encontrado");
            }

            var tarefaExcluida = _mapper.Map<TarefaExcluida>(tarefa);
            tarefaExcluida.DataExclusao = DateTime.UtcNow.AddHours(-3).ToString("dd/MM/yyyy");

            _context.Tarefa_Excluida.Add(tarefaExcluida);
            _context.Tarefa.Remove(tarefa);
            _context.SaveChanges();
            return Result.Ok();
        }

        private double retornaTimer(Tarefa tarefa)
        {
            var anoTarefa = int.Parse(tarefa.DataAlerta.Substring(6, 4));
            var mesTarefa = int.Parse(tarefa.DataAlerta.Substring(3, 2));
            var diaTarefa = int.Parse(tarefa.DataAlerta.Substring(0, 2));
            var horaTarefa = int.Parse(tarefa.HoraAlerta.Substring(0, 2));
            var minTarefa = int.Parse(tarefa.HoraAlerta.Substring(3, 2));

            DateTime a = DateTime.UtcNow.AddHours(-3);
            DateTime b = new DateTime(anoTarefa, mesTarefa, diaTarefa, horaTarefa, minTarefa, 00);
            return b.Subtract(a).TotalMilliseconds;
        }

        private void criaCronometro(double tempo,Tarefa tarefa, string telefone, string nomeAssistido)
        {
            var cronometro = new System.Timers.Timer();
            cronometro.Enabled = false;
            cronometro.Interval = tempo;
            cronometro.AutoReset = false;
            cronometro.Elapsed += async (sender, e)  => enviarMensagemParaRealizarTarefa(tarefa, telefone, nomeAssistido);
            cronometro.Start();
        }

        private void enviarMensagemParaRealizarTarefa (Tarefa tarefa ,string telefone, string nomeAssistido) 
        {      
            _mensagemWpp.enviarMensagemParaRealizarTarefa(tarefa, telefone, nomeAssistido);
        }
    }
}
