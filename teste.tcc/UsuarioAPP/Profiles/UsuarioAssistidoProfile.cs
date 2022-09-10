using APP.Data.Dtos.UsuarioAssistido;
using APP.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Profiles
{
    public class UsuarioAssistidoProfile : Profile
    {
        public UsuarioAssistidoProfile()
        {
            CreateMap<CreateUsuarioAssistido, UsuarioAssistido>();
            CreateMap<UsuarioAssistido, IdentityUser<int>>();
        }
    }
}
