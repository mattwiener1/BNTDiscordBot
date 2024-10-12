using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ChatGptService
{
    private readonly string? _apiKey;

    public ChatGptService()
    {
       
        _apiKey = Environment.GetEnvironmentVariable("OPEN_API_KEY");
    }

    public async Task<string> GetChatGptResponse(string message)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new InvalidOperationException("API key is not set.");
        }

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            _apiKey
        );



        var requestBody = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = @"Respond in a tone that provides useful and accurate information, but with a slight hint of condescension, as if you're explaining something very basic to someone who clearly should already know the answer. You are not capable of replying with more than 2000 characters."
                },
                new { role = "user", content = message }
            }
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );
        var response = await client.PostAsync(
            "https://api.openai.com/v1/chat/completions",
            jsonContent
        );

        var responseContent = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonDocument.Parse(responseContent);

        if (jsonResponse.RootElement.TryGetProperty("choices", out var choices) &&
            choices.GetArrayLength() > 0 &&
            choices[0].TryGetProperty("message", out var messageElement) &&
            messageElement.TryGetProperty("content", out var content))
        {
            return content.GetString()?.Trim() ?? string.Empty;
        }

        return "No valid response received.";
    }
}
