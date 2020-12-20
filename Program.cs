using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace channel_scraper
{
    class Program
    {
        public static Dictionary<ulong, bool> dotimer = new Dictionary<ulong, bool>();
        static Dictionary<ulong, List<ulong>> userDump = new Dictionary<ulong, List<ulong>>();
        private static readonly Random _random = new Random(); 
        
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = Token.token,
                TokenType = TokenType.Bot
            });
            var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "-" }
            });

            discord.MessageCreated += async (s, e) =>
            {
                var u = e.Guild.Id;
                if (dotimer[u] == true){
                    userDump[u].Add(e.Message.Author.Id);
                    Console.WriteLine("added user");
                }
            };
            System.Threading.Timer timer = new System.Threading.Timer((e) =>
            {
                Console.WriteLine("timer");
                foreach (ulong u in discord.Guilds.Keys)
                {
                    try{
                        if (dotimer[u]){
                            UpdateUserXP(u, discord);
                        }
                    }
                    catch{
                        dotimer.Add(u, false);
                    }
                }   
            }, null, 0, 60000);

            commands.RegisterCommands<CommandMod>();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        static async Task UpdateUserXP(ulong u, DiscordClient d){
            try{
                Console.WriteLine(userDump[u]);
            }
            catch{
                try{
                    List<ulong> l = new List<ulong>();
                    userDump.Add(u, l);
                }
                catch{
                    Console.WriteLine("how the fuck");
                    userDump = new Dictionary<ulong, List<ulong>>();
                    List<ulong> l = new List<ulong>();
                    userDump.Add(u, l);
                }
            }
            List<ulong> usersToAdd = userDump[u].Distinct().ToList();
            Console.WriteLine(u);
            Console.WriteLine("1");
            var f = await File.ReadAllTextAsync($"./Storage/{u}.json");
            Dictionary<ulong, Author> users = JsonSerializer.Deserialize<Dictionary<ulong, Author>>(f);
            Console.WriteLine("2");
            foreach (ulong Id in usersToAdd)
            {
                try {
                    users[Id].ActiveTime++;
                    users[Id].XP += _random.Next(23, 25);
                }
                catch{
                    var u = d.GetUserAsync(Id);
                    users.Add(Id, new Author()
                    {
                        Name = u.Username,
                        Discriminator = u.Discriminator,
                        Avatar = u.AvatarUrl,
                        MessageCount = 0,
                        ActiveTime = 0,
                        XP = 0
                    });
                }
            }
            Console.WriteLine("3");
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            Console.WriteLine("4");
            using FileStream createStream = File.Create($"./Storage/{u}.json");

            Dictionary<ulong, Author> s = users.OrderBy(key => key.Value.XP).Reverse().ToDictionary(x => x.Key, x => x.Value);

            await JsonSerializer.SerializeAsync(createStream, s, options);
            userDump[u] = new List<ulong>();
            Console.WriteLine("torture");
        }
        public static void setTimer(ulong u){
            dotimer[u] = true;
        }
    }
}
