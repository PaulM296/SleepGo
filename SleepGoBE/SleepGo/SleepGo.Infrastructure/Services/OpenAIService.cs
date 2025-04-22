using Azure;
using Microsoft.Extensions.Configuration;
using SleepGo.App.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SleepGo.Infrastructure.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> AskQuestionAboutReviewAsync(string question, List<string> reviewTexts)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            var prompt = $"Here are some hotel reviews:\n\n{string.Join("\n\n", reviewTexts)}\n\nBased on these reviews, answer this question: \n\"{question}\"";

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant that summarizes customer reviews." },
                    new { role = "user", content = prompt }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            return jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
    }
}
