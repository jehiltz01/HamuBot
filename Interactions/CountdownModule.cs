using Discord.Interactions;
using HamuBot.Calendar;
using HamuBot.Utilities;

namespace HamuBot.Interactions
{
    /// <summary>
    /// Handles Countdown Slash Commands 
    /// </summary>
    public class CountdownModule : InteractionModuleBase<SocketInteractionContext>
    {
        private SessionTracker sessionLog;
        private readonly CimmerianCalendar cimmerianCalendar;
        const int maxAmount = 100000;

        CountdownModule(CimmerianCalendar cimmerianCalendar)
        {
            this.cimmerianCalendar = cimmerianCalendar;
            //Get SessionLog object
            var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string slJson = File.ReadAllText(outputDir + "/sessionLog.json");
            sessionLog = Newtonsoft.Json.JsonConvert.DeserializeObject<SessionTracker>(slJson);
        }

        [SlashCommand("setcountdown", "Creates a countdown event, counting down from today to x number of days from now")]
        public async Task SetCountdownCmd(string countdownName, int daysAway)
        {
            if (daysAway > maxAmount) {
                await RespondAsync("I'm afraid I can't keep track for that long.");
            }
            sessionLog.CountdownName = countdownName;
            sessionLog.CountdownNumber = daysAway.ToString();
            var dueDate = cimmerianCalendar.IncrementDate(daysAway, true, true);
            UpdateSessionLog();
            await RespondAsync($"I will keep track of the days until {countdownName} for you. You have until {dueDate} to handle it. And remember, take things one day at a time.");
        }

        [SlashCommand("getcountdown", "Reports how much time is left on the current countdown")]
        public async Task GetCountdownCmd()
        {
            if (sessionLog.CountdownName == null || sessionLog.CountdownName == "") {
                await RespondAsync("Rest easy. There are no imminent events on the horizon.");
            } else {
                await RespondAsync($"Be aware, you all have {sessionLog.CountdownNumber} days left until {sessionLog.CountdownName}. I hope you're taking things one day at a time.");
            }
        }

        [SlashCommand("stopcountdown", "Stops the current countdown")]
        public async Task StopCountdownCmd()
        {
            if (sessionLog.CountdownName == null || sessionLog.CountdownName == "") {
                await RespondAsync("There is no countdown to stop.");
            } else {
                var oldCountdown = sessionLog.CountdownName;
                sessionLog.CountdownName = "";
                sessionLog.CountdownNumber = "";
                await RespondAsync($"I will stop the current countdown {oldCountdown}.");
            }
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
