using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

public class ChatGptService
{
    private readonly string _apiKey;

    public ChatGptService()
    {
        _apiKey = Environment.GetEnvironmentVariable("OPEN_API_KEY");
    }

    public async Task<string> GetChatGptResponse(string message)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            _apiKey
        );

        var requestBody = new
        {
            model = "gpt-4o",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = @"Be as helpful as possible at all times. Be frienly, but also sassy."
                },
                new { role = "user", content = message }
            }
        };

        // Add logic to send the request and handle the response
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            System.Text.Encoding.UTF8,
            "application/json"
        );
        var response = await client.PostAsync(
            "https://api.openai.com/v1/chat/completions",
            jsonContent
        );

        var responseContent = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonDocument.Parse(responseContent);
        System.Console.WriteLine(responseContent);
        return jsonResponse
            .RootElement.GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()
            .Trim();
    }
}
