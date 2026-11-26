/*
Nome: Kaik Persike Maiorquino
Prontuário: CB3029689

Nome: Luiz Gustavo Verissimo Monteiro
Prontuário: CB3030326
*/
using System.ComponentModel.DataAnnotations;

namespace CBTSWE2_Prova2.Core.DTOs
{
    public class ProdutoCadastroDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Range(0.01, float.MaxValue,
            ErrorMessage = "O preço deve ser maior que zero.")]
        public float Preco { get; set; }

        public bool Status { get; set; } = true;
    }
}