using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using BNTDiscordBot;

public class DiscordBot
{
    private DiscordSocketClient? _client;
    private string? _discord_app_id;
    private string? _token;
    private Dictionary<string, Func<SocketMessage, Task>> _commandHandlers;

    public DiscordBot()
    {
        _discord_app_id = Environment.GetEnvironmentVariable("DISCORD_APP_ID");

        if (string.IsNullOrEmpty(_discord_app_id))
        {
            throw new ArgumentNullException(nameof(_discord_app_id));
        }
        _token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
        if (string.IsNullOrEmpty(_token))
        {
            throw new ArgumentNullException(nameof(_token));
        }
        _commandHandlers = new Dictionary<string, Func<SocketMessage, Task>>
        {
            { "!roll", RollDice },
            { "!flip", FlipCoin }
        };
    }

    public async Task StartAsync()
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

        // Check if the message is directed at the bot
        if (messageText.Contains($"<@{_discord_app_id}>") || message.Channel is IPrivateChannel)
        {
            foreach (var command in _commandHandlers.Keys)
            {
                if (messageText.Contains(command))
                {
                    await _commandHandlers[command](message);
                    return;
                }
            }
            // Default action if no command matches
            await SendChatGPTMessage(message);
        }
    }

    private async Task SendChatGPTMessage(SocketMessage message)
    {
        var chatGptService = new ChatGptService();
        var response = await chatGptService.GetChatGptResponse(message.Content);
        await message.Channel.SendMessageAsync(response);
    }

    private async Task RollDice(SocketMessage message)
    {
        var messageText = message.Content.ToLower();
        if (int.TryParse(messageText.Replace($"<@{_discord_app_id}> !roll", ""), out var numSides))
        {
            await message.Channel.SendMessageAsync(Dice.Roll(numSides).ToString());
        }
        else
        {
            await message.Channel.SendMessageAsync(Dice.Roll().ToString());
        }
    }

    private async Task FlipCoin(SocketMessage message)
    {
        CoinFlip coinFlip = new CoinFlip();
        var result = coinFlip.Flip();
        await message.Channel.SendMessageAsync(result);
    }
}
