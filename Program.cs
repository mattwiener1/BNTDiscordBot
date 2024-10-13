using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using BNTDiscordBot; 
public class Program
{
    private DiscordSocketClient? _client;
    private string _discord_app_id = Environment.GetEnvironmentVariable("DISCORD_APP_ID");
    private string _token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();

        // Log the bot events for debugging purposes
        _client.Log += LogAsync;
        _client.MessageReceived += MessageReceivedAsync;

        // Login and start the bot
        await _client.LoginAsync(TokenType.Bot, _token);
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
        var messageText = message.Content.ToLower();

        // Respond with a message
        if (messageText.Contains($"<@{_discord_app_id}>"))
        {
            if (messageText.Contains("!roll"))
            {
                await RollDice(messageText, message);
            }
            else
            {
                await SendChatGPTMessage(message);
            }
        }
        else if (message.Channel is IPrivateChannel)
        {
            if (message.Content.ToLower().Contains("!roll"))
            {
                await RollDice(messageText, message);
            }
            else
            {
                await SendChatGPTMessage(message);
            }
        }
    }

    private async Task SendChatGPTMessage(SocketMessage message)
    {
        var chatGptService = new ChatGptService();
        var response = await chatGptService.GetChatGptResponse(message.Content);
        await message.Channel.SendMessageAsync(response);
    }

    private async Task RollDice(string messageText, SocketMessage message)
    {
        int intValue;
        bool successfullyParsed = int.TryParse(messageText.ToLower().Replace($"<@{_discord_app_id}> !roll", "").Trim(), out intValue);
        Console.WriteLine(messageText.ToLower().Replace($"<@{_discord_app_id}> !roll", "").Trim());
        if (successfullyParsed)
        {
            Console.WriteLine("Here");
            await message.Channel.SendMessageAsync(Dice.Roll(intValue).ToString());
        }
        else
        {
            await message.Channel.SendMessageAsync(Dice.Roll().ToString());
        }
    }
}
