namespace MRXLevelBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Bot bot = new Bot();
            await bot.StartAsync();
        }
    }
}