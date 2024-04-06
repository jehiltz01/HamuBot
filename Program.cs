
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using HamuBot.Calendar;
using HamuBot.Interactions;
using HamuBot.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HamuBot
{
    public class Program
    {
        static readonly int msgCacheSize = 200;
        private CimmerianCalendar cimmerianCalendar;
        private ulong channelId;
        private bool testerChannel;

        //channelIDs
        // WoM Server
        private ulong generalChannel = 747151771033665569; // general
        //private ulong generalChannel = 1226200371840417945; // fake-general
        private ulong testingChannel = 822203115272536094; // testing
        // TESTING
        //private ulong generalChannel = 575779513083101191;
        //private ulong testingChannel = 1067291921606774804;

        public static Task Main(string[] args) => new Program().MainAsync(args);

        public async Task MainAsync(string[] args)
        {
            //Create config
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("HamuProverbs.json")
                .Build();

            // Set channelId; if null, default to generalChannel
            if (args.Length > 0 && args[0] == "t") {
                channelId = testingChannel;
                testerChannel = true;
            } else {
                channelId = generalChannel;
                testerChannel = false;
            }

            // Create CimmerianCalendar object
            cimmerianCalendar = new CimmerianCalendar();

            // set up bot configurations
            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                services
                .AddSingleton(config)
                .AddSingleton(cimmerianCalendar)
                .AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig {
                    GatewayIntents = GatewayIntents.MessageContent | GatewayIntents.AllUnprivileged,
                    AlwaysDownloadUsers = true,
                    MessageCacheSize = msgCacheSize
                }))
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionHandler>()
                .AddSingleton(x => new CommandService())
                )
                .Build();

            await RunAsync(host);
        }

        public async Task RunAsync(IHost host)
        {
            // set up service provider 
            // used with dependency injection to retrieve everything from the host 
            using IServiceScope serviceScope = host.Services.CreateScope();
            // provider is what holds the services
            IServiceProvider provider = serviceScope.ServiceProvider;

            // create client
            var client = provider.GetRequiredService<DiscordSocketClient>();
            // Get slash commands from interaction service 
            var slashCommands = provider.GetRequiredService<InteractionService>();
            await provider.GetRequiredService<InteractionHandler>().InitializeAsync();
            //Get config
            var config = provider.GetRequiredService<IConfigurationRoot>();

            // client will have events
            // subscribe to the different events
            client.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };
            slashCommands.Log += async (LogMessage msg) => { Console.WriteLine(msg.Message); };

            client.Ready += async () => {
                Console.WriteLine("Bot Ready");
                //Register slash commands with guild
                await slashCommands.RegisterCommandsToGuildAsync(UInt64.Parse(config["guild"]));
                // Get start session message
                StartSession startSession = new StartSession();
                var startMsg = startSession.GetStartMessage(config, new EmoteManager(), cimmerianCalendar, testerChannel);
                var generalChannel = client.GetChannel(channelId) as IMessageChannel;
                await generalChannel.SendMessageAsync(startMsg);
            };

            //start connection with Discord API
            await client.LoginAsync(TokenType.Bot, config["token"]);
            await client.StartAsync();

            // Wait infinitely so your bot actually stays connected.
            await Task.Delay(Timeout.Infinite);
        }
    }
}