using MauiApp1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    internal class Test
    {
        private readonly HttpClient _httpClient;
        public Test(HttpClient httpClient, string token)
        {
            
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7037");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        public async Task<HttpResponseMessage> LoginAsync(LoginDTO loginDto)
        {

            return await _httpClient.PostAsJsonAsync("/api/Auth/Pobranie", loginDto);
        }
    }
}
