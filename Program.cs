using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Text.Json;

namespace channel_scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = new Token().token,
                TokenType = TokenType.Bot
            });

            var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
            { 
                StringPrefixes = new[] { "-" }
            });

            commands.RegisterCommands<CommandMod>();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
