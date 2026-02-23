using Discord;
using Discord.WebSocket;

namespace MRXLevelBot
{
    public class Bot
    {
        private DiscordSocketClient client = null!;
        private LevelService levelService = null!;

        public async Task StartAsync()
        {
            client = new DiscordSocketClient();
            levelService = new LevelService();

            client.Log += Log;
            client.MessageReceived += MessageReceived;

            var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("DISCORD_TOKEN Bulunamadı!");
                return;
            }

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Author.IsBot) return;

            bool levelUp = levelService.AddXP(message.Author.Id, 10);

            if (levelUp)
            {
                await message.Channel.SendMessageAsync(
                    $"🎉 {message.Author.Username} level atladı! Yeni level: {levelService.GetUser(message.Author.Id).Level}");
            }

            if (message.Content == "!rank")
            {
                var user = levelService.GetUser(message.Author.Id);

                var embed = new EmbedBuilder()
                    .WithTitle($"{message.Author.Username} Rank")
                    .AddField("Level", user.Level, true)
                    .AddField("XP", user.XP, true)
                    .WithColor(Color.Green)
                    .Build();
                
                await message.Channel.SendMessageAsync(embed: embed);
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }
    }
}