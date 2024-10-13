public class Program
{
    public async Task MainAsync()
    {
        var discordBot = new DiscordBot();
        await discordBot.StartAsync();
    }
}
