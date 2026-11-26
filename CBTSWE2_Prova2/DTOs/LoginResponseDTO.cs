/*
Nome: Kaik Persike Maiorquino
Prontuário: CB3029689

Nome: Luiz Gustavo Verissimo Monteiro
Prontuário: CB3030326
*/
namespace CBTSWE2_Prova2.Core.DTOs
{
    public class LoginResponseDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public DateTime Expiracao { get; set; }
    }
}