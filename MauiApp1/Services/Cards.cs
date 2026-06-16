using API.DTOs;
using System.Net.Http.Json;

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

        public async Task<HttpResponseMessage> AddCard(AddCardDTO addCardDTO, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.PostAsJsonAsync("/api/Cards/AddCard", addCardDTO);
        }

        public async Task<HttpResponseMessage> DeleteCard(int cardId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.DeleteAsync($"/api/Cards/DeleteCard/{cardId}");
        }

        public async Task<HttpResponseMessage> UpdateCardName(UpdateCardNameDTO updateCardNameDTO, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.PutAsJsonAsync("/api/Cards/UpdateCardName", updateCardNameDTO);
        }

        public async Task<HttpResponseMessage> DeleteCardsList(int listId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.DeleteAsync($"/api/CardsList/DeleteCardsList/{listId}");
        }

        public async Task<HttpResponseMessage> UpdateCardsListName(UpdateCardsListNameDTO updateCardsListNameDTO, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.PutAsJsonAsync("/api/CardsList/UpdateCardsListName", updateCardsListNameDTO);
        }
    }
}
