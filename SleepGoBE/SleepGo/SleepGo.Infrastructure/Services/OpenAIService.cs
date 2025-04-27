using Microsoft.Extensions.Configuration;
using SleepGo.App.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

public class OpenAIService : IOpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly bool _mockEnabled;

    public OpenAIService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;

        // Read from appsettings.json
        _mockEnabled = configuration.GetValue<bool>("OpenAI:MockMode");
    }

    public async Task<string> AskQuestionAboutReviewAsync(string question, List<string> reviewTexts)
    {
        if (_mockEnabled)
        {
            await Task.Delay(200); // simulate delay

            // VERY basic keyword extraction (for testing purposes only)
            var lowercaseQuestion = question.ToLower();
            var possibleKeywords = new[] { "test", "clean", "ac", "wifi", "hellooo", "rating", "bathroom" };

            string keyword = possibleKeywords
                .FirstOrDefault(k => lowercaseQuestion.Contains(k)) ?? "test";

            var relevant = reviewTexts
                .Where(r => r.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return $"[MOCKED GPT RESPONSE]\nQuestion: {question}\nFound {relevant.Count} matching review(s) based on keyword '{keyword}':\n- " +
                string.Join("\n- ", relevant.Take(3));
        }

        // Real GPT API call
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

        int maxRetries = 3;
        for (int retry = 0; retry < maxRetries; retry++)
        {
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(2000 * (retry + 1)); // exponential backoff
                continue;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"OpenAI API error: {response.StatusCode} - {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            return jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }

        throw new Exception("OpenAI API request failed after multiple retries due to rate limiting.");
    }

    public async Task<string> AskQuestionAboutHotelAsync(string question, List<string> hotelInfos)
    {
        if(_mockEnabled)
        {
            await Task.Delay(200);
            return $"[MOCKED HOTEL GPT RESPONSE]\nQuestion: {question}\nHotels considered: {hotelInfos.Count}";
        }

        var apiKey = _configuration["OpenAI:ApiKey"];
        var prompt = $"Here is a list of hotel information:\n\n{string.Join("\n", hotelInfos)}\n\n" +
            $"Based on this information, answer the following question:\n\"{question}\"";

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant that recommends hotels based on user preferences." },
                new { role = "user", content = prompt }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

        if(!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"OpenAI API error: {response.StatusCode} - {errorContent}");
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(responseBody);

        return jsonDoc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();
    }
}
