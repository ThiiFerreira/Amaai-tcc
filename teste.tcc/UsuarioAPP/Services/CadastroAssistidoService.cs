using APP.Data.Dtos.UsuarioAssistido;
using APP.Models;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Data;

namespace APP.Services
{
    public class CadastroAssistidoService
    {
        private IMapper _mapper;
        private UserManager<IdentityUser<int>> _userManager;
        private RoleManager<IdentityRole<int>> _roleManager;
        private UserDbContext _usuarioService;
        public CadastroAssistidoService(IMapper mapper, UserManager<IdentityUser<int>> userManager, RoleManager<IdentityRole<int>> roleManager, UserDbContext usuarioService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _usuarioService = usuarioService;
        }
        public Result CadastraAssistido(CreateUsuarioAssistido createDto)
        {
            UsuarioAssistido assistido = _mapper.Map<UsuarioAssistido>(createDto);
            Task<IdentityResult> resultadoIdentity;

            try
            {
                var username = _usuarioService.UsuarioAssistido.FirstOrDefault(usuario => usuario.Username == createDto.Username);
                if (username != null) return Result.Fail("Username já existe");

                var usernameResponsavel = _usuarioService.Usuario.FirstOrDefault(usuario => usuario.Username == createDto.Username);
                if (usernameResponsavel != null) return Result.Fail("Username já existe");

                var cpf = _usuarioService.UsuarioAssistido.FirstOrDefault(usuario => usuario.Cpf == createDto.Cpf);
                if (cpf != null) return Result.Fail("CPF já existe");

                var responsavel = _usuarioService.Usuario.FirstOrDefault(usuario => usuario.Id == createDto.ResponsavelId);
                if (responsavel == null) return Result.Fail("Responsavel não existe");

                IdentityUser<int> usuarioIdentity = _mapper.Map<IdentityUser<int>>(assistido);
                resultadoIdentity = _userManager
                    .CreateAsync(usuarioIdentity, createDto.Password);
                _userManager.AddToRoleAsync(usuarioIdentity, "idoso");

                if (resultadoIdentity.Result.Succeeded)
                {

                    var code = _userManager
                        .GenerateEmailConfirmationTokenAsync(usuarioIdentity).Result;
                    assistido.Id = usuarioIdentity.Id;
                    _usuarioService.UsuarioAssistido.Add(assistido);

                    var usuario = _usuarioService.Usuario.FirstOrDefault(usuario => usuario.Id == assistido.ResponsavelId);
                    usuario.IdIdoso = assistido.Id.ToString();
                    _usuarioService.SaveChanges();
                    return Result.Ok().WithSuccess(code);
                }
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
            var erro = resultadoIdentity.Result.ToString();
            if (erro.Contains("Password"))
            {
                return Result.Fail("Senha deve conter 1 Letra maiúscula, 1 caracter especial e 1 número");
            }
            else if (erro.Contains("UserName"))
            {
                return Result.Fail("Username invalido");

            }

            return Result.Fail(erro);
        }
    }
}
