using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using BNTDiscordBot;

public class DiscordBot
{
    private readonly DiscordSocketClient _client;
    private readonly string _discordAppId;
    private readonly string _token;
    private readonly CommandHandler _commandHandler;
    private readonly ChatGptService _chatGptService;

    public DiscordBot()
    {
        _discordAppId = GetEnvironmentVariable("DISCORD_APP_ID");
        _token = GetEnvironmentVariable("DISCORD_BOT_TOKEN");

        _commandHandler = new CommandHandler();
        _commandHandler.RegisterCommand(new RollCommand());
        _commandHandler.RegisterCommand(new FlipCommand());
        _commandHandler.RegisterCommand(new RpsCommand());

        _chatGptService = new ChatGptService();
        _client = new DiscordSocketClient();
    }

    public async Task StartAsync()
    {
        _client.Log += LogAsync;
        _client.MessageReceived += MessageReceivedAsync;

        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();
        await Task.Delay(-1);
    }

    private Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log);
        return Task.CompletedTask;
    }

    private async Task MessageReceivedAsync(SocketMessage message)
    {
        if (message.Author.IsBot) return;

        Console.WriteLine($"Received message: {message.Content}");
        var messageText = message.Content.ToLower();

        if (messageText.Contains($"<@{_discordAppId}>") || message.Channel is IPrivateChannel)
        {
            using (message.Channel.EnterTypingState())
            {
                var commandPrefix = "!";
                if (messageText.StartsWith(commandPrefix))
                {
                    await _commandHandler.HandleCommandAsync(message, commandPrefix);
                }
                else
                {
                    await SendChatGPTMessage(message);
                }
            }
        }
    }

    private async Task SendChatGPTMessage(SocketMessage message)
    {
        var response = await _chatGptService.GetChatGptResponse(message.Content);
        await message.Channel.SendMessageAsync(response);
    }

    private string GetEnvironmentVariable(string variable)
    {
        var value = Environment.GetEnvironmentVariable(variable);
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(variable, $"Environment variable '{variable}' is not set.");
        }
        return value;
    }
}
