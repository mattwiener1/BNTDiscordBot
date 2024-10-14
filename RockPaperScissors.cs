using System;

public static class RockPaperScissors
{
    private static readonly string[] options = { "rock", "paper", "scissors" };
    private static readonly Random random = new Random();

    public static string Shoot()
    {
        int choice = random.Next(options.Length);
        return options[choice];
    }
}
