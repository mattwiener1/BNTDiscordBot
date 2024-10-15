using System.Threading.Tasks;
using Discord.WebSocket;

namespace BNTDiscordBot
{
    public interface ICommand
    {
        string Name { get; }
        Task ExecuteAsync(SocketMessage message);
    }
}
