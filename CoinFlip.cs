namespace BNTDiscordBot;

public class CoinFlip
{
    Random _random;
    List<string> _sides = new List<string>();

    public CoinFlip(){
        _random = new Random();
        _sides.Add("Heads");
        _sides.Add("Tails");
    }

    public string Flip(){
        return _sides[_random.Next(0,2)];
    }
}
