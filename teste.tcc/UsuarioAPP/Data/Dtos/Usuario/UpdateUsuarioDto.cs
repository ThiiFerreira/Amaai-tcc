using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UsuariosApi.Data.Dtos.Usuario
{
    public class UpdateUsuarioDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
    }
}
