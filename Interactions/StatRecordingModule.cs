using Discord.Interactions;
using HamuBot.Enums;
using HamuBot.Utilities;

namespace HamuBot.Interactions
{
    /// <summary>
    /// Handles Stat Recording Slash Commands
    /// </summary>
    public class StatRecordingModule : InteractionModuleBase<SocketInteractionContext>
    {
        private SessionTracker sessionLog;
        private EmoteManager emoteManager;
        const int maxAmount = 100000;
        public StatRecordingModule()
        {
            emoteManager = new EmoteManager();
            //Get SessionLog object
            var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string slJson = File.ReadAllText(outputDir + "/sessionLog.json");
            sessionLog = Newtonsoft.Json.JsonConvert.DeserializeObject<SessionTracker>(slJson);
        }

        [SlashCommand("nat20", "Input player name and, optionally, +/- amount; default amount is 1")]
        public async Task Nat20SlashCmd(NameOptions input, int amount = 1)
        {
            // Get curent value
            var curDieCount = 0;
            try {
                curDieCount = Convert.ToInt32(sessionLog.Nat20s.GetType().GetProperty(input.ToString()).GetValue(sessionLog.Nat20s, null));
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            // Error check
            var totalCount = curDieCount + amount;
            if (totalCount > maxAmount) {
                await RespondAsync("That number is too high I'm afraid.");
                return;
            }
            if (totalCount < 0) {
                await RespondAsync("You must be mistaken, I cannot set this below zero");
                return;
            }
            // Update sessionLog
            sessionLog.Nat20s.GetType().GetProperty(input.ToString()).SetValue(sessionLog.Nat20s, totalCount, null);
            UpdateSessionLog();
            // React to message
            if (amount < 0) {
                if (input.ToString().EndsWith('s')) {
                    await RespondAsync($"Subtracted {Math.Abs(amount)} from {input}' nat20 total :tea:");
                } else {
                    await RespondAsync($"Subtracted {Math.Abs(amount)} from {input}'s nat20 total :tea:");
                }
            } else {
                if (input.ToString().EndsWith('s')) {
                    await RespondAsync($"Added {amount} to {input}' nat20 total :tea:");
                } else {
                    await RespondAsync($"Added {amount} to {input}'s nat20 total :tea:");
                }
            }
        }

        [SlashCommand("nat1", "Input player name and, optionally, +/- amount; default amount is 1")]
        public async Task Nat1SlashCmd(NameOptions input, int amount = 1)
        {
            // Get curent value
            var curDieCount = 0;
            try {
                curDieCount = Convert.ToInt32(sessionLog.Nat1s.GetType().GetProperty(input.ToString()).GetValue(sessionLog.Nat1s, null));
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            // Error check
            var totalCount = curDieCount + amount;
            if (totalCount > maxAmount) {
                await RespondAsync("That number is too high I'm afraid.");
                return;
            }
            if (totalCount < 0) {
                await RespondAsync("You must be mistaken, I cannot set this below zero");
                return;
            }
            // Update sessionLog
            sessionLog.Nat1s.GetType().GetProperty(input.ToString()).SetValue(sessionLog.Nat1s, totalCount, null);
            UpdateSessionLog();
            // React to message
            if (amount < 0) {
                if (input.ToString().EndsWith('s')) {
                    await RespondAsync($"Subtracted {Math.Abs(amount)} from {input}' nat1 total :tea:");
                } else {
                    await RespondAsync($"Subtracted {Math.Abs(amount)} from {input}'s nat1 total :tea:");
                }
            } else {
                if (input.ToString().EndsWith('s')) {
                    await RespondAsync($"Added {amount} to {input}' nat1 total :tea:");
                } else {
                    await RespondAsync($"Added {amount} to {input}'s nat1 total :tea:");
                }
            }
        }

        [SlashCommand("kills", "Input player name and, optionally, +/- amount; default amount is 1")]
        public async Task KillSlashCmd(NameOptionsNoDM input, int amount = 1)
        {
            // Get curent value
            var curDieCount = 0;
            try {
                curDieCount = Convert.ToInt32(sessionLog.Kills.GetType().GetProperty(input.ToString()).GetValue(sessionLog.Kills, null));
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            // Error check
            var totalCount = curDieCount + amount;
            if (totalCount > maxAmount) {
                await RespondAsync("That number is too high I'm afraid.");
                return;
            }
            if (totalCount < 0) {
                await RespondAsync("You must be mistaken, I cannot set this below zero");
                return;
            }
            // Update sessionLog
            sessionLog.Kills.GetType().GetProperty(input.ToString()).SetValue(sessionLog.Kills, totalCount, null);
            UpdateSessionLog();
            // React to message
            if (amount < 0) {
                if (input.ToString().EndsWith('s')) {
                    await RespondAsync($"Subtracted {Math.Abs(amount)} from {input}' kills total :tea:");
                } else {
                    await RespondAsync($"Subtracted {Math.Abs(amount)} from {input}'s kills total :tea:");
                }
            } else {
                if (input.ToString().EndsWith('s')) {
                    await RespondAsync($"Added {amount} to {input}' kills total :tea:");
                } else {
                    await RespondAsync($"Added {amount} to {input}'s kills total :tea:");
                }
            }
        }

        [SlashCommand("bosskills", "Input player name and, optionally, +/- amount; default amount is 1")]
        public async Task BossKillSlashCmd(NameOptionsNoDM input, int amount = 1)
        {
            // Get curent value
            var curDieCount = 0;
            try {
                curDieCount = Convert.ToInt32(sessionLog.BossKills.GetType().GetProperty(input.ToString()).GetValue(sessionLog.BossKills, null));
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            // Error check
            var totalCount = curDieCount + amount;
            if (totalCount > maxAmount) {
                await RespondAsync("That number is too high I'm afraid.");
                return;
            }
            if (totalCount < 0) {
                await RespondAsync("You must be mistaken, I cannot set this below zero");
                return;
            }
            // Update sessionLog
            sessionLog.BossKills.GetType().GetProperty(input.ToString()).SetValue(sessionLog.BossKills, totalCount, null);
            UpdateSessionLog();
            // React to message
            if (amount < 0) {
                if (input.ToString().EndsWith('s')) {
                    await RespondAsync($"Subtracted {Math.Abs(amount)} from {input}' boss kills total :tea:");
                } else {
                    await RespondAsync($"Subtracted {Math.Abs(amount)} from {input}'s boss kills total :tea:");
                }
            } else {
                if (input.ToString().EndsWith('s')) {
                    await RespondAsync($"Added {amount} to {input}' boss kills total :tea:");
                } else {
                    await RespondAsync($"Added {amount} to {input}'s boss kills total :tea:");
                }
            }
        }

        [SlashCommand("downed", "Input player name and, optionally, +/- amount; default amount is 1")]
        public async Task DownedSlashCmd(NameOptionsNoDM input, int amount = 1)
        {
            // Get curent value
            var curDieCount = 0;
            try {
                curDieCount = Convert.ToInt32(sessionLog.Downed.GetType().GetProperty(input.ToString()).GetValue(sessionLog.Downed, null));
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            // Error check
            var totalCount = curDieCount + amount;
            if (totalCount > maxAmount) {
                await RespondAsync("That number is too high I'm afraid.");
                return;
            }
            if (totalCount < 0) {
                await RespondAsync("You must be mistaken, I cannot set this below zero");
                return;
            }
            // Update sessionLog
            sessionLog.Downed.GetType().GetProperty(input.ToString()).SetValue(sessionLog.Downed, totalCount, null);
            UpdateSessionLog();
            // React to message
            if (amount < 0) {
                if (input.ToString().EndsWith('s')) {
                    await RespondAsync($"Subtracted {Math.Abs(amount)} from {input}' downed total :tea:");
                } else {
                    await RespondAsync($"Subtracted {Math.Abs(amount)} from {input}'s downed total :tea:");
                }
            } else {
                if (input.ToString().EndsWith('s')) {
                    await RespondAsync($"Added {amount} to {input}' downed total :tea:");
                } else {
                    await RespondAsync($"Added {amount} to {input}'s downed total :tea:");
                }
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
