using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

public class Program
{
    private DiscordSocketClient _client;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();

        // Log the bot events for debugging purposes
        _client.Log += LogAsync;
        _client.MessageReceived += MessageReceivedAsync;

        // Insert your bot's token here
        var token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");

        // Login and start the bot
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Keep the bot running
        await Task.Delay(-1);
    }

    // Logging any bot event information
    private Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log.ToString());
        return Task.CompletedTask;
    }

    // This method is triggered whenever a message is received
    private async Task MessageReceivedAsync(SocketMessage message)
    {
        // Don't reply to the bot's own messages
        if (message.Author.IsBot) return;

        // Log the received message
        Console.WriteLine($"Received message: {message.Content}");

        // Respond with a message
        if (message.Content.Contains("<@1294610193249996921>"))
        {
            var chatGptService = new ChatGptService();
            // System.Console.WriteLine(message.Content.Substring(22));
            var messageSubstring = message.Content.Substring(22);
            var response = await chatGptService.GetChatGptResponse(messageSubstring);

            if (response.Length > 2000){
                var strings = SplitChunks(response, 2000);
                foreach (var s in strings)
                {
                    System.Console.WriteLine(s);
                    await message.Channel.SendMessageAsync(s);
                    
                }

            }
            else{
                await message.Channel.SendMessageAsync(response);
                
            }
        }
    }
    static IEnumerable<string> SplitChunks(string str, int chunkSize)
{
    return Enumerable.Range(0, str.Length / chunkSize)
        .Select(i => str.Substring(i * chunkSize, chunkSize));
}

}


