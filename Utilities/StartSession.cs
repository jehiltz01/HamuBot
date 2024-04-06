using HamuBot.Calendar;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace HamuBot.Utilities
{
    public class StartSession
    {
        /// <summary>
        /// Gets the opening message that starts a session
        /// </summary>
        /// <param name="config"></param>
        /// <param name="emoteManager"></param>
        /// <param name="cimmerianCalendar"></param>
        /// <param name="testerChannel"></param>
        /// <returns></returns>
        public string GetStartMessage(IConfigurationRoot config, EmoteManager emoteManager, CimmerianCalendar cimmerianCalendar, bool testerChannel)
        {
            // get session log
            var sessionLog = new SessionTracker();
            try {
                var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                string slJson = File.ReadAllText(outputDir + "/sessionLog.json");
                sessionLog = Newtonsoft.Json.JsonConvert.DeserializeObject<SessionTracker>(slJson);
            } catch (Exception ex) {
                Console.WriteLine("Failed to process sessionLog.json in StartSession");
            }
            // Build start session message
            StringBuilder startMsg = new StringBuilder();
            startMsg.AppendLine($"Welcome to the World of Midnight! {emoteManager.GetEmote("Eclipse")}");
            startMsg.AppendLine("~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~");
            startMsg.AppendLine(cimmerianCalendar.GetCurrentDate());
            // Send opening message, either generated or custom
            if (sessionLog.CustomOpening == "") {
                if (testerChannel) {
                    startMsg.AppendLine($"Why hello there. :chess_pawn:");
                } else {
                    startMsg.AppendLine(GenerateProverb(config));
                }
            } else {
                startMsg.AppendLine($"{sessionLog.CustomOpening}");
            }
            // Post reminder, if there is one
            if (sessionLog.Reminder != "") {
                startMsg.AppendLine("~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~");
                startMsg.AppendLine($"You all requested that I pass along this message from your past selves: {sessionLog.Reminder}");
            }
            // If we're in the testing channel, don't reset the reminder or custom opening 
            if (!testerChannel) {
                sessionLog.CustomOpening = "";
                sessionLog.Reminder = "";
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(sessionLog, Newtonsoft.Json.Formatting.Indented);
                try {
                    var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                    File.WriteAllText(outputDir + "/sessionLog.json", output);
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
            return startMsg.ToString();
        }

        /// <summary>
        /// Gets a random line from the HamuProverbs file
        /// </summary>
        /// <param name="config"></param>
        /// <returns>A random musing as a string</returns>
        private string GenerateProverb(IConfigurationRoot config)
        {
            StringBuilder finalProverb = new StringBuilder();

            try {
                //Get HamuProverbs object
                var proverbs = new HamuProverbs();
                var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                string mmJson = File.ReadAllText(outputDir + "/HamuProverbs.json");
                proverbs = Newtonsoft.Json.JsonConvert.DeserializeObject<HamuProverbs>(mmJson);
                // Generate random opening and random proverb
                var random = new Random();
                finalProverb.Append(proverbs.Openers[random.Next(proverbs.Openers.Length)]);
                finalProverb.AppendLine(proverbs.Proverbs[random.Next(proverbs.Proverbs.Length)]);
            } catch (Exception e) {
                Console.WriteLine("Error generating musings");
            }
            return finalProverb.ToString();
        }
    }
}
