namespace HamuBot.Services
{
    public class BotShutdownService
    {
        private readonly Func<Task> _shutdownCallback;

        public BotShutdownService(Func<Task> shutdownCallback)
        {
            _shutdownCallback = shutdownCallback;
        }

        public Task ShutdownAsync() => _shutdownCallback();
    }
}
