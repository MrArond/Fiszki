using API.DTOs;
using MauiApp1.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class AuthClient
    {
        private readonly HttpClient _httpClient;

        public AuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7037");
        }

        public async Task<HttpResponseMessage> RegisterAsync(RegisterDTO registerDto)
        {

            return await _httpClient.PostAsJsonAsync("/api/Auth/Register", registerDto);
        }

        public async Task<HttpResponseMessage> LoginAsync(LoginDTO loginDto)
        {
            return await _httpClient.PostAsJsonAsync("/api/Auth/Login", loginDto);
        }

        public async Task<HttpResponseMessage> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO)
        {
            return await _httpClient.PostAsJsonAsync("/api/Auth/ForgotPassword", forgotPasswordDTO);
        }

        public async Task<HttpResponseMessage> AddCardsList(AddCardsListDTO addCardsListDTO, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.PostAsJsonAsync("/api/CardsList/AddCardsList", addCardsListDTO);
        }

        public async Task<HttpResponseMessage> GetUserCardsLists(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetAsync("/api/CardsList/GetUserCardsLists");
        }
    }
}

