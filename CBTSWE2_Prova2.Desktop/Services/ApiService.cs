/*
Nome: Kaik Persike Maiorquino
Prontuário: CB3029689

Nome: Luiz Gustavo Verissimo Monteiro
Prontuário: CB3030326
*/
using System.Net.Http;
using System.Net.Http.Headers;

namespace CBTSWE2_Prova2.Desktop.Services
{
    public static class ApiService
    {
        public static HttpClient Client { get; } = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7084/")
        };

        public static void DefinirToken(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public static void RemoverToken()
        {
            Client.DefaultRequestHeaders.Authorization = null;
        }
    }
}