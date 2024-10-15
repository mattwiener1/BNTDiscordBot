using System.Threading.Tasks;
using Discord.WebSocket;

namespace BNTDiscordBot
{
    public class RollCommand : ICommand
    {
        public string Name => "roll";

        public async Task ExecuteAsync(SocketMessage message)
        {
            var messageText = message.Content.ToLower().Replace("!roll", "").Trim();
            if (int.TryParse(messageText, out var numSides))
            {
                await message.Channel.SendMessageAsync(Dice.Roll(numSides).ToString());
            }
            else
            {
                await message.Channel.SendMessageAsync(Dice.Roll().ToString());
            }
        }
    }
}
