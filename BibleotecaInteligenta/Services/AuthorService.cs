using BibleotecaInteligenta.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BibleotecaInteligenta.Services
{
    public class AuthorService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7126";
        private readonly AuthService _authService;

        public AuthorService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        // POST
        public async Task<CreateAuthorDTO> CreateAuthor(CreateAuthorDTO author)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Authors", author);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CreateAuthorDTO>();
        }

        // GET BY ID
        public async Task<AuthorDTO> GetAuthor(long id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/Authors?id={id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AuthorDTO>();
        }

        // GET LIST OF AUTHORS
        public async Task<List<AuthorDTO>> GetAuthors()
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/Authors/list");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<AuthorDTO>>();
        }

        // PUT
        public async Task EditAuthor(AuthorDTO author)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/Authors", author);
            response.EnsureSuccessStatusCode();
        }

        // DELETE
        public async Task DeleteAuthor(long id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Authors/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}