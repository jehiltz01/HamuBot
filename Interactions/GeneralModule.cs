using Discord;
using Discord.Interactions;
using HamuBot.Services;
using HamuBot.Utilities;

namespace HamuBot.Interactions
{
    /// <summary>
    /// Handles General Slash Commands
    /// </summary>
    public class GeneralModule : InteractionModuleBase<SocketInteractionContext>
    {
        private SessionTracker sessionLog;
        // WoM Server
        private ulong testingChannelId = 822203115272536094; // testing
        // TESTING
        //private ulong testingChannel = 1067291921606774804;

         private readonly BotShutdownService _shutdownService;

        public GeneralModule(BotShutdownService shutdownService)
        {
            //Get SessionLog object
            var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string slJson = File.ReadAllText(outputDir + "/sessionLog.json");
            sessionLog = Newtonsoft.Json.JsonConvert.DeserializeObject<SessionTracker>(slJson);

            _shutdownService = shutdownService;
        }

        [SlashCommand("endsession", "Ends the session")]
        public async Task EndSessionCmd()
        {
            // Respond with custom sign off or standard message
            if (sessionLog.CustomSignoff != "") {
                await RespondAsync(sessionLog.CustomSignoff);
                // If command is sent in the tester channel, don't reset the custom sign off 
                if (Context.Channel.Id != testingChannelId) {
                    sessionLog.CustomSignoff = "";
                    UpdateSessionLog();
                }
            } else {
                await RespondAsync("Good session everyone! I look forward to our next meeting. :tea:");
            }

            // Send backup sessionLog file to testing channel
            var testingChannel = Context.Client.GetChannel(testingChannelId) as IMessageChannel;
            var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            await testingChannel.SendFileAsync(outputDir + "/sessionLog.json");

            //wait for messages to send before closing connection
            await Task.Delay(1000);

            await _shutdownService.ShutdownAsync();
        }

        [SlashCommand("setreminder", "Sets a reminder for next session")]
        public async Task SetReminderCmd(string reminder, bool manual = false)
        {
            sessionLog.Reminder = reminder;
            UpdateSessionLog();
            await RespondAsync("I will remind you of this when you need it most.");
        }

        /// <summary>
        /// Updates sessionLog.json
        /// </summary>
        private void UpdateSessionLog()
        {
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(sessionLog, Newtonsoft.Json.Formatting.Indented);
            try {
                var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                File.WriteAllText(outputDir + "/sessionLog.json", output);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
