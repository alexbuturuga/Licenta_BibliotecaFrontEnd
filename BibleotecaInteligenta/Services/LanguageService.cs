using BibleotecaInteligenta.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BibleotecaInteligenta.Services
{
    public class LanguageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7126";
        private readonly AuthService _authService;

        public LanguageService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        // POST
        public async Task<CreateLanguageDTO> CreateLanguage(CreateLanguageDTO language)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Language", language);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CreateLanguageDTO>();
        }

        // GET BY ID
        public async Task<LanguageDTO> GetLanguage(long id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/Language?id={id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<LanguageDTO>();
        }

        // GET LIST OF LANGUAGES
        public async Task<List<LanguageDTO>> GetLanguages()
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/Language/list");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<LanguageDTO>>();
        }

        // PUT
        public async Task EditLanguage(LanguageDTO language)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/Language", language);
            response.EnsureSuccessStatusCode();
        }

        // DELETE
        public async Task DeleteLanguage(long id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Language/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
