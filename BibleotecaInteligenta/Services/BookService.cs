using BibleotecaInteligenta.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BibleotecaInteligenta.Services
{
    public class BookService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7126";
        private readonly AuthService _authService;

        public BookService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        // POST
        public async Task<CreateBookDTO> CreateBook(CreateBookDTO Book)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Books", Book);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CreateBookDTO>();
        }

        // GET BY ID
        public async Task<BookDTO> GetBook(int id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/Books?id={id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BookDTO>();
        }

        // GET LIST OF BOOKS
        public async Task<List<BookDTO>> GetBooks()
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/Books/list");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<BookDTO>>();
        }

        // GET LIST OF BOOKS
        public async Task<List<PopularBookDTO>> GetPopularBooks()
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/Books/popularList");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<PopularBookDTO>>();
        }

        // PUT
        public async Task EditBook(CreateBookDTO Book)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/Books", Book);
            response.EnsureSuccessStatusCode();
        }

        // DELETE
        public async Task DeleteBook(long id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Books?id={id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
