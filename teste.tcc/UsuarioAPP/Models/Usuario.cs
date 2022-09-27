
using System.ComponentModel.DataAnnotations;

namespace UsuariosApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public bool Tem_idoso { get; set; }
    }
}
