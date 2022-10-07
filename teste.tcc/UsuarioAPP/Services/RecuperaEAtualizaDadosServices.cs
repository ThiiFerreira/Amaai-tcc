﻿using APP.Models;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Data;
using UsuariosApi.Data.Dtos.Usuario;
using UsuariosApi.Data.Dtos.UsuarioAssistido;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class RecuperaEAtualizaDadosServices
    {
        private UserDbContext _context;

        public RecuperaEAtualizaDadosServices(UserDbContext context)
        {
            _context = context;
        }

        public Usuario RecuperaDadosUsuarioPorId(int id)
        {
            Usuario usuario = _context.Usuario.FirstOrDefault(usuario => usuario.Id == id);
            if (usuario != null)
            {
                return usuario;
            }
            return null;
        }

        public UsuarioAssistido RecuperaDadosUsuarioAssistidoPorIdDoResponsavel(int id)
        {
            var usuario = _context.UsuarioAssistido.FirstOrDefault(usuario => usuario.ResponsavelId == id);
            if (usuario != null)
            {
                return usuario;
            }
            return null;
        }

        public IdentityUser<int> RecuperaDadosUserPorId(int id)
        {
            var  usuario = _context.Users.FirstOrDefault(usuario => usuario.Id == id);
            if (usuario != null)
            {
                return usuario;
            }
            return null;
        }

        public Result AtualizaDadosResponsavel(UpdateUsuarioDto usuarioDto, int id)
        {
            var user = _context.Users.FirstOrDefault(usuario => usuario.Id == id);
            var usuario = _context.Usuario.FirstOrDefault(usuario => usuario.Id == id);

            if (user == null || usuario == null)
            {
                return Result.Fail("user não encontrada");
            }

            if (usuarioDto.Username != null)
            {
                user.UserName = usuarioDto.Username;
                user.NormalizedUserName = usuarioDto.Username.ToUpper();
                usuario.Username = usuarioDto.Username;
            }
            if (usuarioDto.Email != null)
            {
                user.Email = usuarioDto.Email;
                user.NormalizedEmail = usuarioDto.Email.ToUpper();
                usuario.Email = usuarioDto.Email;
            }
            if (usuarioDto.Nome != null)
            {
                usuario.Nome = usuarioDto.Nome;
            }
            if (usuarioDto.Cpf != null)
            {
                usuario.Cpf = usuarioDto.Cpf;
            }
            if (usuarioDto.DataNascimento != null)
            {
                usuario.DataNascimento = usuarioDto.DataNascimento;

            }
            if (usuarioDto.Telefone != null)
            {
                usuario.Telefone = usuarioDto.Telefone;

            }
            if (usuarioDto.Endereco != null)
            {
                usuario.Endereco = usuarioDto.Endereco;
            }
            _context.SaveChanges();
            return Result.Ok();
        }

        public string RecuperaContatoUsuarioAssistidoPorIdDoSeuResponsavel(int id)
        {
            var usuarioAssistido = _context.UsuarioAssistido.FirstOrDefault(usuario => usuario.ResponsavelId == id);
         
            if(usuarioAssistido != null)
            {
                return usuarioAssistido.Telefone;
            }
            return null; 
        }

        public Result AtualizaDadosUsuarioAssistido(UpdateUsuarioAssitidoDto usuarioDto, int id)
        {
            
            var usuario = _context.UsuarioAssistido.FirstOrDefault(usuario => usuario.ResponsavelId == id);
            var user = _context.Users.FirstOrDefault(user => user.Id == usuario.Id);

            if (user == null || usuario == null)
            {
                return Result.Fail("user não encontrada");
            }

            if (usuarioDto.Username != null)
            {
                user.UserName = usuarioDto.Username;
                user.NormalizedUserName = usuarioDto.Username.ToUpper();
                usuario.Username = usuarioDto.Username;
            }
            if (usuarioDto.Nome != null)
            {
                usuario.Nome = usuarioDto.Nome;
            }
            if (usuarioDto.Cpf != null)
            {
                usuario.Cpf = usuarioDto.Cpf;
            }
            if (usuarioDto.DataNascimento != null)
            {
                usuario.DataNascimento = usuarioDto.DataNascimento;

            }
            if (usuarioDto.Telefone != null)
            {
                usuario.Telefone = usuarioDto.Telefone;

            }
            if (usuarioDto.Endereco != null)
            {
                usuario.Endereco = usuarioDto.Endereco;
            }
            _context.SaveChanges();
            return Result.Ok();
        }
    }
}
