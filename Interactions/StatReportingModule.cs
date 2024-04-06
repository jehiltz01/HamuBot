using Discord;
using Discord.Interactions;
using HamuBot.Utilities;
using System.Text;

namespace HamuBot.Interactions
{
    /// <summary>
    /// Handles Stat Reporting Slash Commands
    /// </summary>
    public class StatReportingModule : InteractionModuleBase<SocketInteractionContext>
    {
        private SessionTracker sessionLog;

        public StatReportingModule()
        {
            //Get SessionLog object
            var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string slJson = File.ReadAllText(outputDir + "/sessionLog.json");
            sessionLog = Newtonsoft.Json.JsonConvert.DeserializeObject<SessionTracker>(slJson);
        }

        [SlashCommand("critreport", "Shows the number of nat20s and nat1s every player has rolled")]
        public async Task CritReportCmd()
        {
            StringBuilder critReport = new StringBuilder();
            critReport.AppendLine("## Crit Report");
            critReport.AppendLine();
            critReport.AppendLine(GetCritReport());
            await RespondAsync(critReport.ToString());
        }

        [SlashCommand("combatreport", "Shows various combat stats such as kill count and how many times someone has been knocked down")]
        public async Task CombatReportCmd()
        {
            StringBuilder combatReport = new StringBuilder();
            combatReport.AppendLine("## Combat Report");
            combatReport.AppendLine();
            combatReport.AppendLine(GetCombatReport());
            await RespondAsync(combatReport.ToString());
        }

        [SlashCommand("fullreport", "Shows all recorded game statistics")]
        public async Task FullReportCmd()
        {
            // Get channel id from where command came from 
            var curChannel = Context.Client.GetChannel(Context.Channel.Id) as IMessageChannel;

            // Respond directly and then send additional messages for crit and combat reports
            await RespondAsync("## Full Report");
            await curChannel.SendMessageAsync(GetCritReport());
            await curChannel.SendMessageAsync(GetCombatReport());
            return;
        }

        /// <summary> 
        /// Gets the reports for nat20s and nat1s 
        /// </summary>
        /// <returns></returns>
        private string GetCritReport()
        {
            var statReporter = new StatReporter(sessionLog);
            var nat20Report = statReporter.GetNat20Report();
            var nat1Report = statReporter.GetNat1Report();
            StringBuilder critReport = new StringBuilder();
            critReport.AppendLine(nat20Report);
            critReport.AppendLine(nat1Report);

            return critReport.ToString();
        }

        /// <summary>
        /// Gets the reports for kills (including boss kills) and downed
        /// </summary>
        /// <returns></returns>
        private string GetCombatReport()
        {
            var statReporter = new StatReporter(sessionLog);
            var killReport = statReporter.GetKillsReport();
            var downedReport = statReporter.GetDownedReport();
            StringBuilder combatReport = new StringBuilder();
            combatReport.AppendLine(killReport);
            combatReport.AppendLine(downedReport);

            return combatReport.ToString();
        }
    }
}
