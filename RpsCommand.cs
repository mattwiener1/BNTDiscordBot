using System.Threading.Tasks;
using Discord.WebSocket;

namespace BNTDiscordBot
{
    public class RpsCommand : ICommand
    {
        public string Name => "rps";

        public async Task ExecuteAsync(SocketMessage message)
        {
            await message.Channel.SendMessageAsync(RockPaperScissors.Shoot());
        }
    }
}
