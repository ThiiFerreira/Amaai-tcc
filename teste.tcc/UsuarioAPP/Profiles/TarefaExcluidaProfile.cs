using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Data.Dtos.Tarefa;
using UsuariosApi.Models;

namespace UsuariosApi.Profiles
{
    public class TarefaExcluidaProfile : Profile
    {
        public TarefaExcluidaProfile()
        {
            CreateMap<TarefaExcluida, ReadTarefaExcluidaDto>();
            CreateMap<Tarefa, TarefaExcluida>();
        }
    }
}
