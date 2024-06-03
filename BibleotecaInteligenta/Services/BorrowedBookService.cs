using BibleotecaInteligenta.DTOs;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BibleotecaInteligenta.Services
{
    public class BorrowedBookService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7126";
        private readonly AuthService _authService;

        public BorrowedBookService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        // POST
        public async Task<BorrowedBookDTO> CreateBorrowedBook(BorrowedBookDTO borrowedBook)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/BorrowedBooks", borrowedBook);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BorrowedBookDTO>();
        }

        // GET BY ID
        public async Task<BorrowedBookDTO> GetBorrowedBook(long id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/BorrowedBooks?id={id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BorrowedBookDTO>();
        }

        // GET LIST OF BORROWED BOOKS
        public async Task<List<BorrowedBookDTO>> GetBorrowedBooks()
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/BorrowedBooks/list");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<BorrowedBookDTO>>();
        }

        // GET LIST OF BORROWED BOOKS BY USER ID
        public async Task<List<BorrowedBookDTO>> GetBorrowedBooksByUserId(long userId)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/BorrowedBooks/listByUser?userId={userId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<BorrowedBookDTO>>();
        }

        // PUT
        public async Task EditBorrowedBook(BorrowedBookDTO borrowedBook)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/BorrowedBooks", borrowedBook);
            response.EnsureSuccessStatusCode();
        }

        // CONFIRM
        public async Task ConfirmBorrowedBook(long id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/BorrowedBooks/confirmed", id);
            response.EnsureSuccessStatusCode();
        }

        // DELETE
        public async Task DeleteBorrowedBook(long id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_baseUrl}/api/BorrowedBooks?Id={id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
