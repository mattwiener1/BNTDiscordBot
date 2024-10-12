using System;
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
        if (message.Content.Contains("!hello"))
        {
            await message.Channel.SendMessageAsync("Hello! I'm your friendly bot.");
        }
    }
}
