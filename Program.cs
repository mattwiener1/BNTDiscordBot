public class Program
{
    public static async Task Main()
    {
        var discordBot = new DiscordBot();
        await discordBot.StartAsync();
    }
}
