using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Data.Dtos.Tarefa;
using UsuariosApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace UsuariosApi.Services
{
    public class TarefaServiceArray
    {
        private IMapper _mapper;
        public List<Tarefa> listTarefa { get; set; }
        public TarefaServiceArray(IMapper mapper)
        {
            _mapper = mapper;
            listTarefa = AddTarefas();
        }

        public List<Tarefa> AddTarefas()
        {
            List<Tarefa> list = new List<Tarefa>();

            var tarefa = new Tarefa();
            tarefa.Id = 1;
            tarefa.Descricao = "Tarefa 1";
            tarefa.DataInicio = "01/01/0001";
            tarefa.DataFinal = "01/01/0001";
            tarefa.ResponsavelId = 00001;
            tarefa.IdosoId = 00002;
            list.Add(tarefa);

            var tarefa2 = new Tarefa();
            tarefa2.Id = 2;
            tarefa2.Descricao = "Tarefa 2";
            tarefa2.DataInicio = "01/01/0001";
            tarefa2.DataFinal = "01/01/0001";
            tarefa2.ResponsavelId = 00001;
            tarefa2 .IdosoId = 00002;
            list.Add(tarefa2);


            var tarefa3 = new Tarefa();
            tarefa3.Id = 3;
            tarefa3.Descricao = "Tarefa 3";
            tarefa3.DataInicio = "01/01/0001";
            tarefa3.DataFinal = "01/01/0001";
            tarefa3.ResponsavelId = 00001;
            tarefa3.IdosoId = 00002;
            list.Add(tarefa3);


            var tarefa4 = new Tarefa();
            tarefa4.Id = 4;
            tarefa4.Descricao = "Tarefa 4";
            tarefa4.DataInicio = "01/01/0001";
            tarefa4.DataFinal = "01/01/0001";
            tarefa4.ResponsavelId = 00001;
            tarefa4.IdosoId = 00002;
            list.Add(tarefa4);


            var tarefa5 = new Tarefa();
            tarefa5.Id = 5;
            tarefa5.Descricao = "Tarefa 5";
            tarefa5.DataInicio = "01/01/0001";
            tarefa5.DataFinal = "01/01/0001";
            tarefa5.ResponsavelId = 00001;
            tarefa5.IdosoId = 00002;
            list.Add(tarefa5);

            return list;
        }

        
        public ReadTarefaDto RecuperaTarefaPorId(int id)
        {
            Tarefa tarefa = listTarefa.FirstOrDefault(tarefa => tarefa.Id == id);
            if (tarefa != null)
            {
                return _mapper.Map<ReadTarefaDto>(tarefa);
            }
            return null;
        }

        public List<ReadTarefaDto> RecuperaTarefa()
        {
            
            if (listTarefa != null)
            {
                return _mapper.Map<List<ReadTarefaDto>>(listTarefa);
            }
            return null;
        }    
    }
}
