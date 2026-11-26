/*
Nome: Kaik Persike Maiorquino
Prontuário: CB3029689

Nome: Luiz Gustavo Verissimo Monteiro
Prontuário: CB3030326
*/
using System.ComponentModel.DataAnnotations;

namespace CBTSWE2_Prova2.Core.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; } = string.Empty;
    }
}