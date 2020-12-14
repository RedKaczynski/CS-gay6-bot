/*if (e.Message.Content.ToLower().StartsWith("--scrape")) {
var channels = e.Message.Guild.Channels.AsEnumerable().Select(n => n.Value).Where(channel => channel.Type == ChannelType.Text).ToList();
await e.Message.RespondAsync("Scraping...");
var i = 0;
foreach (DiscordChannel channel in channels)
{
    var currBefore = ctx.Message.Id;
    var scrapedCount = 0;
    while (true)
    {
        var messages = await channel.GetMessagesBeforeAsync(currBefore);
        scrapedCount += messages.Count;
        if (messages.Count == 0) break;
        else currBefore = messages[messages.Count - 1].Id;
        await db.Messages.AddRangeAsync(messages.AsEnumerable().Select(m =>
            new Message { Id = m.Id, Author = GetDisplayName(m.Author), Content = m.Content, Channel = m.Channel.Name }));
    }
    await e.Message.RespondAsync($"✅ Scraped {scrapedCount} messages from {channel.Mention} (Channel {i + 1} of {channels.Count})");
    await db.SaveChangesAsync();
    await Task.Delay(1000);
    i++;
}
await ctx.RespondAsync("✅ Finished");*/
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

public class CommandMod : BaseCommandModule
{
    Dictionary<long, string> authors = new Dictionary<long, string>();
    [Command("scrape")]
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
                //Console.WriteLine(scrapedCount);
                //await db.Messages.AddRangeAsync(messages.AsEnumerable().Select(m =>
                //    new Message { Id = m.Id, Author = GetDisplayName(m.Author), Content = m.Content, Channel = m.Channel.Name }));
            }
            await ctx.RespondAsync($"✅ Scraped {scrapedCount} messages from {channel.Mention} (Channel {i + 1} of {channels.Count})");
            //await db.SaveChangesAsync();
            await Task.Delay(1000);
            i++;
        }
        await ctx.RespondAsync("✅ Finished");
    }
}
