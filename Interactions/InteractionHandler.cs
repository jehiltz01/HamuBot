using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

namespace HamuBot.Interactions
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient client;
        private readonly InteractionService commands;
        private readonly IServiceProvider services;

        public InteractionHandler(DiscordSocketClient client, InteractionService commands, IServiceProvider services)
        {
            this.client = client;
            this.commands = commands;
            this.services = services;
        }

        //Add slash command modules
        //Subscribe to the interaction created event that discord sends out when someone uses an interaction 
        public async Task InitializeAsync()
        {
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);

            client.InteractionCreated += HandleInteraction;
        }

        /// <summary>
        /// Handles interactions, such as prefix and slash commands
        /// </summary>
        /// <param name="arg"> supplied by the Interaction Created event the client subscribed to in InitializeAsync</param>
        /// <returns></returns>
        private async Task HandleInteraction(SocketInteraction arg)
        {
            //Create context used in the interactions
            try {
                var context = new SocketInteractionContext(client, arg);
                await commands.ExecuteCommandAsync(context, services);
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
