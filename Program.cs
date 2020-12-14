using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;

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
                Token = "Nzg3NTg4Mzc2MjExNjE5ODYy.X9XI1Q.Od8vIGqwfPUsTBb2snOSpfgLZnw",
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
