/*
Nome: Kaik Persike Maiorquino
Prontuário: CB3029689

Nome: Luiz Gustavo Verissimo Monteiro
Prontuário: CB3030326
*/
using System.ComponentModel.DataAnnotations;

namespace CBTSWE2_Prova2.Core.DTOs
{
    public class UsuarioCadastroDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve possuir pelo menos 6 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        public bool Status { get; set; } = true;
    }
}