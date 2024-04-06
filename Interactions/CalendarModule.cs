using Discord.Interactions;
using HamuBot.Calendar;
using HamuBot.Utilities;
using System.Text;

namespace HamuBot.Interactions
{
    /// <summary>
    /// Handles Calendar/Date related Slash Commands
    /// </summary>
    public class CalendarModule : InteractionModuleBase<SocketInteractionContext>
    {
        private SessionTracker sessionLog;
        private readonly CimmerianCalendar cimmerianCalendar;
        private EmoteManager emoteManager;
        const int maxAmount = 100000;
        const int numMonths = 12;
        const int numDays = 28;

        public CalendarModule(CimmerianCalendar cimmerianCalendar)
        {
            this.cimmerianCalendar = cimmerianCalendar;
            emoteManager = new EmoteManager();
            //Get SessionLog object
            var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string slJson = File.ReadAllText(outputDir + "/sessionLog.json");
            sessionLog = Newtonsoft.Json.JsonConvert.DeserializeObject<SessionTracker>(slJson);
        }

        [SlashCommand("todaysdate", "Announces today's date")]
        public async Task TodaysDateCmd()
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine("Today's date is:");
            msg.AppendLine(cimmerianCalendar.GetCurrentDate());
            await RespondAsync(msg.ToString());
        }

        [SlashCommand("setdate", "Updates today's date with a new month and day")]
        public async Task SetDateCmd(int month, int day)
        {
            if (month > numMonths || month < 1 || day > numDays || day < 1) {
                await RespondAsync("This date eludes me. Please try again.");
            }
            cimmerianCalendar.SetDate(month, day);
            await RespondAsync($"We successfully set the date to {month}/{day}");
        }

        [SlashCommand("nextday", "Update today's date to tomorrow's date")]
        public async Task NextDayCmd()
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine("Today is now:");
            msg.AppendLine(cimmerianCalendar.IncrementDate(1, false, false));
            await RespondAsync(msg.ToString());
        }

        [SlashCommand("increasedate", "Shifts today's date by a designated amount")]
        public async Task IncreaseDateCmd(int amount)
        {
            if (amount < 0) {
                await RespondAsync("My apologies, but I'm unable to go back in time");
            } else if (amount == 0) {
                await RespondAsync("Your request confuses me. Increasing the date by zero would simply result in... today.");
            } else if (amount > maxAmount) {
                await RespondAsync("My apologies, but I'm unable to skip over that much valuable time.");
            } else {
                var msg = new StringBuilder();
                msg.AppendLine("Today is now:");
                msg.AppendLine(cimmerianCalendar.IncrementDate(amount, false, false));
                await RespondAsync(msg.ToString());
            }
        }

        [SlashCommand("forecast", "Details a date x amount of days into the future")]
        public async Task ForecastCmd(int amount)
        {
            if (amount < 0) {
                await RespondAsync("It's wise to not dwell on the past.");
            } else if (amount == 0) {
                await RespondAsync("Today is today. I shouldn't need to tell you that.");
            } else if (amount > maxAmount) {
                await RespondAsync("My apologies, I'm unable to predict a day that far in advance.");
            } else {
                var msg = new StringBuilder();
                if (amount == 1) {
                    msg.AppendLine($"I believe the date {amount} day into the future will be:");
                } else {
                    msg.AppendLine($"I believe the date {amount} days into the future will be:");
                }
                msg.AppendLine(cimmerianCalendar.IncrementDate(amount, true, false));
                await RespondAsync(msg.ToString());
            }

        }
    }
}
