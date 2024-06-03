using BibleotecaInteligenta.DTOs;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BibleotecaInteligenta.Services
{
    public class AuthService
    {
        private string? Token;
        private DateTime TokenExpiration;
        private readonly HttpClient _httpClient;

        public async Task<bool> Authenticate(string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                LoginDTO loginDto = new LoginDTO
                {
                    UserName = username,
                    Password = password
                };

                HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7099/api/auth/login", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsAsync<ResponseType>();

                    if (responseData != null)
                    {
                        Token = responseData.Token;
                        TokenExpiration = responseData.Expiration;

                        return true;
                    }
                }
                return false;
            }
        }

        public async Task<HttpResponseMessage> MakeAuthenticatedRequest(HttpMethod method, string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var request = new HttpRequestMessage(method, url);
                return await client.SendAsync(request);
            }
        }

        public string? GetToken()
        {
            return Token;
        }

        public DateTime GetTokenExpiration()
        {
            return TokenExpiration;
        }

        public void SetToken(string token, DateTime expiration)
        {
            Token = token;
            TokenExpiration = expiration;
        }
    }
}
