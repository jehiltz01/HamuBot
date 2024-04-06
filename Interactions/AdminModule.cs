using Discord;
using Discord.Interactions;
using HamuBot.Calendar;
using HamuBot.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Text;

namespace HamuBot.Interactions
{
    /// <summary>
    /// Handles Slash Commands that are restricted to Admin Only
    /// </summary>
    public class AdminModule : InteractionModuleBase<SocketInteractionContext>
    {
        // WoM Server
        private ulong testingChannelId = 822203115272536094; // testing
        // TESTING
        //private ulong testingChannel = 1067291921606774804;

        private readonly CimmerianCalendar cimmerianCalendar;
        private SessionTracker sessionLog;
        private EmoteManager emoteManager;

        public AdminModule(CimmerianCalendar cimmerianCalendar)
        {
            this.cimmerianCalendar = cimmerianCalendar;
            emoteManager = new EmoteManager();
            //Get SessionLog object
            var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string slJson = File.ReadAllText(outputDir + "/sessionLog.json");
            sessionLog = Newtonsoft.Json.JsonConvert.DeserializeObject<SessionTracker>(slJson);
        }

        [SlashCommand("setcustomsaying", "Sets a custom starting message for the following session; will be erased after said session")]
        public async Task SetCustomSayingCmd(string message)
        {
            var processedMsg = message.Replace("//", "\n");
            sessionLog.CustomOpening = processedMsg;
            UpdateSessionLog();
            await RespondAsync(emoteManager.GetEmote("Eclipse"));
        }

        [SlashCommand("setcustomsignoff", "Sets a custom sign off message for the current session; will be erased after said session")]
        public async Task SetCustomSignoffCmd(string message)
        {
            var processedMsg = message.Replace("//", "\n");
            sessionLog.CustomSignoff = processedMsg;
            UpdateSessionLog();
            await RespondAsync(emoteManager.GetEmote("Eclipse"));
        }

        [SlashCommand("updatesessionlog", "Overwrites sessionLog.json with new .json file that was sent right before this in the chat")]
        public async Task UpdateSessionLogCmd()
        {
            // Get message sent right before this command 
            var messages = await Context.Channel.GetMessagesAsync(1).FlattenAsync();
            var lastMsg = messages.First<IMessage>();
            // Base error checking
            if (lastMsg.Attachments.Count <= 0) {
                await RespondAsync("Could not find file in previous message. Please try again by uploading sessionLog.json and then entering this command again.");
                return;
            }
            var file = lastMsg.Attachments.ElementAt(0);
            if (file.Filename != "sessionLog.json") {
                await RespondAsync("Incorrect file name. Please try again by uploading sessionLog.json and then entering this command again.");
                return;
            }
            string fileContents = string.Empty;
            // Try to read file
            try {
                HttpClient httpClient = new HttpClient();
                byte[] buffer = await httpClient.GetByteArrayAsync(file.Url);
                fileContents = Encoding.UTF8.GetString(buffer);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                await RespondAsync($"Failed to read or download sessionLog.json. Error: {e.Message}");
                return;
            }
            // Ensure file format is correct
            HamuSchemas hSchemas = new HamuSchemas();
            JObject newSL = JObject.Parse(fileContents);
            var slSchema = hSchemas.GetSessionLogSchema();
            if (!newSL.IsValid(slSchema)) {
                await RespondAsync("File is not formatted correctly. Please ensure that the new sessionLog.json file follows the formatting standards of current sessionLog.json");
                return;
            }
            // Update/overwrite current sessionLog.json 
            try {
                var outputFile = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/sessionLog.json";
                if (File.Exists(outputFile)) {
                    File.WriteAllText(outputFile, fileContents);
                } else {
                    await RespondAsync($"Cannot find current SessionLog.json.");
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                await RespondAsync($"Failed to update current sessionLog.json. Error: {e.Message}");
            }
            await RespondAsync("Successfully updated sessionLog.json :tea:");
        }

        [SlashCommand("updatehamuproverbs", "Overwrites HamuProverbs.json with new .json file that was sent right before this in the chat")]
        public async Task UpdateHamuProverbsCmd()
        {
            // Get message sent right before this command 
            var messages = await Context.Channel.GetMessagesAsync(1).FlattenAsync();
            var lastMsg = messages.First<IMessage>();
            // Base error checking
            if (lastMsg.Attachments.Count <= 0) {
                await RespondAsync("Could not find file in previous message. Please try again by uploading HamuProverbs.json and then entering this command again.");
                return;
            }
            var file = lastMsg.Attachments.ElementAt(0);
            if (file.Filename != "HamuProverbs.json") {
                await RespondAsync("Incorrect file name. Please try again by uploading HamuProverbs.json and then entering this command again.");
                return;
            }
            string fileContents = string.Empty;
            // Try to read file
            try {
                HttpClient httpClient = new HttpClient();
                byte[] buffer = await httpClient.GetByteArrayAsync(file.Url);
                fileContents = Encoding.UTF8.GetString(buffer);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                await RespondAsync($"Failed to read or download HamuProverbs.json. Error: {e.Message}");
                return;
            }
            // Ensure file format is correct
            HamuSchemas jSchemas = new HamuSchemas();
            JObject newHP = JObject.Parse(fileContents);
            var mmSchema = jSchemas.GetHamuProverbsSchema();
            if (!newHP.IsValid(mmSchema)) {
                await RespondAsync("File is not formatted correctly. Please ensure that the new HamuProverbs.json file follows the formatting standards of current HamuProverbs.json");
                return;
            }
            // Update/overwrite current HamuProverbs.json 
            try {
                var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                File.WriteAllText(outputDir + "/HamuProverbs.json", fileContents);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                await RespondAsync($"Failed to update current HamuProverbs.json. Error: {e.Message}");
            }
            await RespondAsync("Successfully updated HamuProverbs.json :tea:");
        }

        [SlashCommand("updatecalendar", "Overwrites CimmerianCalendar.csv with new .csv file that was sent right before this in the chat")]
        public async Task UpdateCimmerianCalendarCmd()
        {
            // Get message sent right before this command 
            var messages = await Context.Channel.GetMessagesAsync(1).FlattenAsync();
            var lastMsg = messages.First<IMessage>();
            // Base error checking
            if (lastMsg.Attachments.Count <= 0) {
                await RespondAsync("Could not find file in previous message. Please try again by uploading CimmerianCalendar.csv and then entering this command again.");
                return;
            }
            var file = lastMsg.Attachments.ElementAt(0);
            if (file.Filename != "CimmerianCalendar.csv") {
                await RespondAsync("Incorrect file name. Please try again by uploading CimmerianCalendar.csv and then entering this command again. Ensure that the file type is csv (comma separated values).");
                return;
            }
            string fileContents = string.Empty;
            // Try to read file
            try {
                HttpClient httpClient = new HttpClient();
                byte[] buffer = await httpClient.GetByteArrayAsync(file.Url);
                fileContents = Encoding.UTF8.GetString(buffer);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                await RespondAsync($"Failed to read or download CimmerianCalendar.csv. Error: {e.Message}");
                return;
            }
            // Ensure file format is correct
            if (!IsValidCalendarFile(fileContents)) {
                await RespondAsync("File is not formatted correctly. Please ensure that the new CimmerianCalendar.csv file follows the formatting standards of current CimmerianCalendar.csv");
                return;
            }
            // Update/overwrite current CimmerianCalendar.csv 
            try {
                var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                File.WriteAllText(outputDir + "/CimmerianCalendar.csv", fileContents);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                await RespondAsync($"Failed to update current CimmerianCalendar.csv. Error: {e.Message}");
            }
            await RespondAsync("Successfully updated CimmerianCalendar.csv :tea:");
        }

        [SlashCommand("getallfiles", "Outputs the sessionLog.json, HamuProverbs.json, and CimmerianCalendar.csv")]
        public async Task GetAllFilesCmd()
        {
            await RespondAsync("Data files coming right up! :tea:");

            // Send files to testing channel
            var testingChannel = Context.Client.GetChannel(testingChannelId) as IMessageChannel;
            var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            await testingChannel.SendFileAsync(outputDir + "/sessionLog.json");
            await testingChannel.SendFileAsync(outputDir + "/HamuProverbs.json");
            await testingChannel.SendFileAsync(outputDir + "/CimmerianCalendar.csv");
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

        /// <summary>
        /// Checks if an input csv matches the required formatting of a CimmerianCalendar file
        /// </summary>
        /// <param name="calendarFile"></param>
        /// <returns></returns>
        private bool IsValidCalendarFile(string calendarFile)
        {
            // Validation constants
            var numRows = 337;
            var numCols = 6;
            var dateCols = 4; // number of columns for the date (values cannot be null in those spots)
            var header = new string[] { "Month (Number)", "Month (Name)", "Day", "Days of the Week", "Birthdays", "Holiday" };
            // split by row
            string[] calendarRows = calendarFile.Split("\r\n");
            // check rows
            // file should have 337 lines but excel adds a blank row at the bottom
            if (calendarRows.Length != numRows + 1) {
                Console.WriteLine($"Calendar file has incorrect number of rows: {calendarRows.Length}. Should be {numRows}. This accounts for the last blank row of the file that is added by excel");
                return false;
            }
            // remove blank row
            calendarRows = calendarRows.Take(numRows).ToArray();
            // Initialize calendar 2d array
            var calendar = new string[numRows, numCols];
            // Fill in calendar
            for (int i = 0; i < numRows; i++) {
                var col = calendarRows[i].Split(",");
                for (int j = 0; j < numCols; j++) {
                    calendar[i, j] = col[j];
                }
            }
            // check header
            if (calendar.GetLength(1) != numCols) {
                Console.WriteLine($"Calendar files has incorrect number of columns: {calendar.GetLength(1)}. Should be {numCols}");
                return false;
            }
            // check for rogue BOM marker (aka ? at start)
            calendar[0, 0] = calendar[0, 0].Replace("\uFEFF", "");
            for (int i = 0; i < numCols; i++) {
                if (calendar[0, i].ToString() != header[i]) {
                    Console.WriteLine($"Calendar has invalid header name: {calendar[0, i]} when it should be: {header[i]}");
                    return false;
                }
            }
            // ensure dates aren't null
            for (int i = 1; i < numRows - 1; i++) {
                for (int j = 0; j < dateCols; j++) {
                    if (calendar[i, j].ToString() == "") {
                        Console.WriteLine("Calendar is missing date value(s)");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
