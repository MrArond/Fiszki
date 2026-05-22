using API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class Cards
    {
        private readonly HttpClient _httpClient;

        public Cards(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7037");
        }

        public async Task<HttpResponseMessage> GetUserCardsLists(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetAsync("/api/CardsList/GetUserCardsLists");
        }

        public async Task<HttpResponseMessage> AddCardsList(AddCardsListDTO addCardsListDTO, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.PostAsJsonAsync("/api/CardsList/AddCardsList", addCardsListDTO);
        }
        public async Task<HttpResponseMessage> GetCardsByListId(int listId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetAsync($"/api/Cards/GetCards/{listId}");
        }
    }
}
