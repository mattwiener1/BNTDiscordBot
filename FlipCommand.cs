using System.Threading.Tasks;
using Discord.WebSocket;

namespace BNTDiscordBot
{
    public class FlipCommand : ICommand
    {
        public string Name => "flip";

        public async Task ExecuteAsync(SocketMessage message)
        {
            await message.Channel.SendMessageAsync(CoinFlip.Flip());
        }
    }
}
