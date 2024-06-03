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
    public class ReviewService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7126";
        private readonly AuthService _authService;

        public ReviewService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        // POST
        public async Task<ReviewDTO> CreateReview(ReviewDTO review)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Reviews", review);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReviewDTO>();
        }

        // GET BY ID
        public async Task<ReviewDTO> GetReview(int id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/Reviews?id={id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReviewDTO>();
        }

        // GET LIST OF REVIEWS
        public async Task<List<ReviewDTO>> GetReviews()
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/Reviews/list");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ReviewDTO>>();
        }

        // GET LIST OF REVIEWS BY BOOK ID
        public async Task<List<ReviewDTO>> GetReviewsByBookId(long bookId)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/Reviews/bookId?bookId={bookId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ReviewDTO>>();
        }



        // PUT
        public async Task EditReview(ReviewDTO review)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/Reviews", review);
            response.EnsureSuccessStatusCode();
        }

        // DELETE
        public async Task DeleteReview(long id)
        {
            string token = _authService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Reviews?Id={id}");
            response.EnsureSuccessStatusCode();
        }
    }
}