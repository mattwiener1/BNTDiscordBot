using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

public class Program
{
    
    private DiscordSocketClient? _client;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        
        var token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");

        _client = new DiscordSocketClient();

        // Log the bot events for debugging purposes
        _client.Log += LogAsync;
        _client.MessageReceived += MessageReceivedAsync;

        // Insert your bot's token here

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
        var discord_app_id = Environment.GetEnvironmentVariable("DISCORD_APP_ID");

        // Respond with a message
        if (message.Content.Contains($"<@{discord_app_id}>"))
        {
            var chatGptService = new ChatGptService();
            var messageSubstring = message.Content.Substring(22);
            var response = await chatGptService.GetChatGptResponse(messageSubstring);
            await message.Channel.SendMessageAsync(response);
        }
    }
}
