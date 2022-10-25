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
    public class TarefaProfile : Profile
    {
        public TarefaProfile()
        {
            CreateMap<CreateTarefaDto, Tarefa>();
            CreateMap<Tarefa, ReadTarefaDto>();
        }
    }
}
