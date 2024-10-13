namespace BNTDiscordBot;

public class CoinFlip
{
    private static Random _random = new Random();
    private static List<string> _sides = new List<string> { "Heads", "Tails" };

    public static string Flip()
    {
        return _sides[_random.Next(0, 2)];
    }
}
