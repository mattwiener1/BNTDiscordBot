using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace BNTDiscordBot
{
    public class CommandHandler
    {
        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        public void RegisterCommand(ICommand command)
        {
            _commands[command.Name.ToLower()] = command;
        }

        public async Task HandleCommandAsync(SocketMessage message, string prefix)
        {
            var content = message.Content.ToLower();
            if (!content.StartsWith(prefix)) return;

            var commandName = content.Split(' ')[0].Substring(prefix.Length);

            if (_commands.TryGetValue(commandName, out var command))
            {
                await command.ExecuteAsync(message);
            }
        }

        public bool HasCommand(string commandName)
        {
            return _commands.ContainsKey(commandName.ToLower());
        }
    }
}
