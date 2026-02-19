using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MauiApp1.DTOs;

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
    }
}

