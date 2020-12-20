using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

public class CommandMod : BaseCommandModule
{
    private readonly Random _random = new Random(); 
    public Dictionary<ulong, Author> authors = new Dictionary<ulong, Author>();

    [Command("fullreset")]
    public async Task ScrapeCommand(CommandContext ctx)
    {
        var channels = ctx.Guild.Channels.AsEnumerable().Select(n => n.Value).Where(channel => channel.Type == ChannelType.Text).ToList();
        await ctx.RespondAsync("Scraping...");
        var i = 0;
        foreach (DiscordChannel channel in channels)
        {
            var currBefore = ctx.Message.Id;
            await ctx.RespondAsync($"now scraping{channel.Mention}");
            var scrapedCount = 0;
            while (true)
            {
                var messages = await channel.GetMessagesBeforeAsync(currBefore);
                scrapedCount += messages.Count;
                if (messages.Count == 0) break;
                else currBefore = messages[messages.Count - 1].Id;
                Console.WriteLine(scrapedCount);
                foreach (DiscordMessage message in messages){
                    if (message.Author.IsBot == true) continue;
                    try{
                        authors[message.Author.Id].messages.Add(new Message(){
                            Id = message.Id,
                            Channel = message.Channel.Name,
                            Time = message.Timestamp.ToUnixTimeMilliseconds(),
                            Content = message.Content
                        });
                    } catch (Exception e){
                        authors.Add(message.Author.Id, new Author()
                        {
                            Name = message.Author.Username,
                            Discriminator = message.Author.Discriminator,
                            Avatar = message.Author.AvatarUrl,
                            MessageCount = 0,
                            ActiveTime = 0,
                            XP = 0,
                            messages = new List<Message>()
                        });
                        authors[message.Author.Id].messages.Add(new Message(){
                            Id = message.Id,
                            Channel = message.Channel.Name,
                            Time = message.Timestamp.ToUnixTimeMilliseconds(),
                            Content = message.Content
                        });
                    }
                }
            }
            await ctx.RespondAsync($"✅ Scraped {scrapedCount} messages from {channel.Mention} (Channel {i + 1} of {channels.Count})");
            //await db.SaveChangesAsync();
            i++;
        }
        i = 0;
        foreach (DiscordChannel channel in channels)
        {
            var currBefore = ctx.Message.Id;
            var scrapedCount = 0;
            var messages = await channel.GetMessagesAfterAsync(currBefore, 10000);
            foreach (DiscordMessage message in messages){
                if (message.Author.IsBot == true) continue;
                try{
                    authors[message.Author.Id].messages.Add(new Message(){
                        Id = message.Id,
                        Channel = message.Channel.Name,
                        Time = message.Timestamp.ToUnixTimeMilliseconds(),
                        Content = message.Content
                    });
                } catch (Exception e){
                    authors.Add(message.Author.Id, new Author()
                    {
                        Name = message.Author.Username,
                        Discriminator = message.Author.Discriminator,
                        Avatar = message.Author.AvatarUrl,
                        MessageCount = 0,
                        ActiveTime = 0,
                        XP = 0,
                        messages = new List<Message>()
                    });
                    authors[message.Author.Id].messages.Add(new Message(){
                        Id = message.Id,
                        Channel = message.Channel.Name,
                        Time = message.Timestamp.ToUnixTimeMilliseconds(),
                        Content = message.Content
                    });
                }
            }
            await ctx.RespondAsync($"✅ Scraped the rest of the messages ({scrapedCount} messages) from {channel.Mention} (Channel {i + 1} of {channels.Count})");
            i++;
        }
        await ctx.RespondAsync($"Calculating XP for {authors.Count} Users...");
        foreach (Author user in authors.Values)
        {
            var messages = user.messages.OrderBy(x => x.Time);
            long Last = 0;
            foreach (Message m in messages)
            {
                user.MessageCount++;
                if(m.Time > Last + 60000){
                    user.ActiveTime++;
                    user.XP += _random.Next(20, 25);
                }
            }
        }

        await ctx.RespondAsync("Saving messages to file...");
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        using FileStream createStream = File.Create($"./Storage/{ctx.Guild.Id}.json");

        Dictionary<ulong, Author> s = authors.OrderBy(key => key.Value.XP).Reverse().ToDictionary(x => x.Key, x => x.Value);

        await JsonSerializer.SerializeAsync(createStream, s, options);
        
        await ctx.RespondAsync("✅ Finished");
        channel_scraper.Program.setTimer(ctx.Guild.Id);
    }
    [Command("kill")]
    public async Task End(CommandContext ctx){
        System.Environment.Exit(0);
    }
    [Command("start")]
    public async Task Start(CommandContext ctx){
        Console.Write("started");
        channel_scraper.Program.setTimer(ctx.Guild.Id);
    }
}
