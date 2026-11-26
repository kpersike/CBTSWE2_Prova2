/*
Nome: Kaik Persike Maiorquino
Prontuário: CB3029689

Nome: Luiz Gustavo Verissimo Monteiro
Prontuário: CB3030326
*/
namespace CBTSWE2_Prova2.Core.DTOs
{
    public class ProdutoResponseDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public float Preco { get; set; }

        public bool Status { get; set; }

        public int IdUsuarioCadastro { get; set; }

        public int IdUsuarioUpdate { get; set; }
    }
}