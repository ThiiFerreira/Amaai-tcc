using APP.Models;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Data;
using UsuariosApi.Data.Dtos.Usuario;
using UsuariosApi.Data.Dtos.UsuarioAssistido;
using UsuariosApi.Data.Requests;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class RecuperaEAtualizaDadosServices
    {
        private UserDbContext _context;
        private SignInManager<IdentityUser<int>> _signInManager;


        public RecuperaEAtualizaDadosServices(UserDbContext context, SignInManager<IdentityUser<int>> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
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

        public Result ExcluirUsuarioResponsavel(LoginRequest request)
        {
            var resultadoIdentity = _signInManager
                .PasswordSignInAsync(request.Username, request.Password, false, false);
            if (!resultadoIdentity.Result.Succeeded)
            {
                return Result.Fail("Falha ao excluir usuario");
            }

            var identityUser = _signInManager
                    .UserManager
                    .Users
                    .FirstOrDefault(usuario =>
                    usuario.NormalizedUserName == request.Username.ToUpper());
            var id = identityUser.Id;

            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            var usuario = _context.Usuario.FirstOrDefault(x => x.Id == id);
            var assistido = _context.UsuarioAssistido.FirstOrDefault(x => x.ResponsavelId == id);
            if (user == null || usuario == null)
            {
                return Result.Fail("Falha ao excluir usuario");
            }

            if(assistido != null)
            {
                return Result.Fail("Não é possivel excluir conta com assistido cadastrado");
            }

            _context.Users.Remove(user);
            _context.Usuario.Remove(usuario);
            _context.SaveChanges();

            return Result.Ok();
        }

        public Result ExcluirUsuarioAssistido(LoginRequest request)
        {
            var resultadoIdentity = _signInManager
                .PasswordSignInAsync(request.Username, request.Password, false, false);
            if (!resultadoIdentity.Result.Succeeded)
            {
                return Result.Fail("Falha ao excluir usuario");
            }

            var identityUser = _signInManager
                    .UserManager
                    .Users
                    .FirstOrDefault(usuario =>
                    usuario.NormalizedUserName == request.Username.ToUpper());
            var id = identityUser.Id;
            var usuario = _context.Usuario.FirstOrDefault(x => x.Id == id);
            var assistido = _context.UsuarioAssistido.FirstOrDefault(x => x.ResponsavelId == id);

            if (assistido == null)
            {
                return Result.Fail("Falha ao excluir conta");
            }

            var tarefa = _context.Tarefa.FirstOrDefault(x => x.ResponsavelId == id);

            if(tarefa != null)
            {
                return Result.Fail("Impossivel excluir conta com tarefas existentes, apague as tarefas e tente novamente");

            }


            usuario.IdIdoso = null;
            _context.UsuarioAssistido.Remove(assistido);

            _context.SaveChanges();
            return Result.Ok();
        }
    }
}
