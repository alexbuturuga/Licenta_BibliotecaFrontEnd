using BibleotecaInteligenta.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BibleotecaInteligenta.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7126";
        private readonly AuthService _authService;

        public UserService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        // POST
        public async Task<UserDTO> CreateUser(UserDTO user)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Users", user);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDTO>();
        }

        // GET BY ID
        public async Task<UserDTO> GetUser(string id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/Users?id={id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDTO>();
        }

        // PUT
        public async Task EditUser(UserEditDTO user)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/Users", user);
            response.EnsureSuccessStatusCode();
        }

        // DELETE
        public async Task DeleteUser(string id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Users/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
