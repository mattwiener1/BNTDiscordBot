using Microsoft.VisualBasic;

namespace BNTDiscordBot;

public static class Dice
{
    static readonly int _sides = 20;

    public static int Roll()
    {
        Random random = new Random();
        return random.Next(1, _sides + 1);
    }
    public static int Roll(int sides)
    {
        Random random = new Random();
        return random.Next(1, sides + 1);
    }
}
