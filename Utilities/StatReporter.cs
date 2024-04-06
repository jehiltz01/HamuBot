using HamuBot.Enums;
using System.Text;

namespace HamuBot.Utilities
{
    public class StatReporter
    {
        private readonly SessionTracker sessionLog;
        private EmoteManager emoteManager;

        public StatReporter(SessionTracker sessionLog)
        {
            this.sessionLog = sessionLog;
            emoteManager = new EmoteManager();
        }

        /// <summary>
        /// Concatenates a message containing the nat20 stat report
        /// </summary>
        /// <returns></returns>
        public string GetNat20Report()
        {
            List<PlayerStat> playerStats = CollectStats(StatOptions.Nat20s.ToString());
            StringBuilder nat20Report = new StringBuilder();

            var total = GetStatTotal(playerStats);
            nat20Report.AppendLine($"The total amount of {emoteManager.GetEmote("Nat20")}s to date is {total}");

            // Order player list in descending order (exclude DM and Guest)
            var pcStats = playerStats.Skip(1).Take(playerStats.Count - 2).ToList();
            var orderedPCStats = from ps in pcStats
                                 orderby ps.DieCount descending
                                 select ps;
            // Player stats
            nat20Report.AppendLine("Critical successes per lucky player are as follows: ");
            foreach (PlayerStat pc in orderedPCStats) {
                var msg = (pc.DieCount == 1) ?
                    $"{pc.PlayerEmote} with {pc.DieCount} nat 20" :
                    $"{pc.PlayerEmote} with {pc.DieCount} nat 20s";
                nat20Report.AppendLine(msg);
            }
            // DM stats
            var dmStat = playerStats[0];
            var dmMsg = (dmStat.DieCount == 1) ?
                $"The {dmStat.PlayerEmote} has rolled {dmStat.DieCount} nat 20" :
                $"The {dmStat.PlayerEmote} has rolled {dmStat.DieCount} nat 20s";
            nat20Report.AppendLine(dmMsg);
            // Guest Stats
            var guestStat = playerStats[playerStats.Count - 1];
            var guestMsg = (guestStat.DieCount == 1) ?
                $"And our {guestStat.PlayerEmote} have rolled {guestStat.DieCount} nat 20" :
                $"And our {guestStat.PlayerEmote} have rolled {guestStat.DieCount} nat 20s";
            nat20Report.AppendLine(guestMsg);
            // Final stats
            return nat20Report.ToString();
        }

        /// <summary>
        /// Concatenates a message containing the nat1 stat report
        /// </summary>
        /// <returns></returns>
        public string GetNat1Report()
        {
            List<PlayerStat> playerStats = CollectStats(StatOptions.Nat1s.ToString());
            StringBuilder nat1Report = new StringBuilder();

            var total = GetStatTotal(playerStats);
            nat1Report.AppendLine($"The total amount of {emoteManager.GetEmote("Nat1")}s to date is {total}");

            // Order player list in descending order (exclude DM and Guest)
            var pcStats = playerStats.Skip(1).Take(playerStats.Count - 2).ToList();
            var orderedPCStats = from ps in pcStats
                                 orderby ps.DieCount descending
                                 select ps;
            // Player stats
            nat1Report.AppendLine("Critical failures per unlucky player are as follows: ");
            foreach (PlayerStat pc in orderedPCStats) {
                var msg = (pc.DieCount == 1) ?
                    $"{pc.PlayerEmote} with {pc.DieCount} nat 1" :
                    $"{pc.PlayerEmote} with {pc.DieCount} nat 1s";
                nat1Report.AppendLine(msg);
            }
            // DM stats
            var dmStat = playerStats[0];
            var dmMsg = (dmStat.DieCount == 1) ?
                $"The {dmStat.PlayerEmote} has rolled {dmStat.DieCount} nat 1" :
                $"The {dmStat.PlayerEmote} has rolled {dmStat.DieCount} nat 1s";
            nat1Report.AppendLine(dmMsg);
            // Guest Stats
            var guestStat = playerStats[playerStats.Count - 1];
            var guestMsg = (guestStat.DieCount == 1) ?
                $"And our {guestStat.PlayerEmote} have rolled {guestStat.DieCount} nat 1" :
                $"And our {guestStat.PlayerEmote} have rolled {guestStat.DieCount} nat 1s";
            nat1Report.AppendLine(guestMsg);
            // Final stats
            return nat1Report.ToString();
        }

        /// <summary>
        /// Concatenates a message containing the kill and boss kill stat report
        /// </summary>
        /// <returns></returns>
        public string GetKillsReport()
        {
            List<PlayerStat> playerKillStats = CollectStats(StatOptions.Kills.ToString());
            List<PlayerStat> playerBossKillStats = CollectStats(StatOptions.BossKills.ToString());
            StringBuilder killReport = new StringBuilder();

            var total = GetStatTotal(playerKillStats);
            var totalBoss = GetStatTotal(playerBossKillStats);
            killReport.AppendLine($"The total amount of kills to date is {total}");
            killReport.AppendLine($"The total amount of boss kills to date is {totalBoss}");

            // Create new list that adds together normal and boss kill counts
            List<PlayerStat> playerTotalKillStats = CollectStats(StatOptions.Kills.ToString());
            for (int i = 0; i < playerTotalKillStats.Count; i++) {
                playerTotalKillStats[i].DieCount = playerKillStats[i].DieCount + playerBossKillStats[i].DieCount;
            }

            // Order player list in descending order (exclude DM and Guest)
            var pcStats = playerTotalKillStats.Skip(1).Take(playerTotalKillStats.Count - 2).ToList();
            var orderedPCStats = from ps in pcStats
                                 orderby ps.DieCount descending
                                 select ps;
            // Player stats
            killReport.AppendLine("Number of kills per player are as follows: ");
            foreach (PlayerStat pc in orderedPCStats) {
                // kills
                var kills = playerKillStats.First(item => item.PlayerName == pc.PlayerName).DieCount;
                var msgKill = (kills == 1) ?
                    $"{pc.PlayerEmote} with {kills} kill" :
                    $"{pc.PlayerEmote} with {kills} kills";
                // boss kills
                var bossKills = playerBossKillStats.First(item => item.PlayerName == pc.PlayerName).DieCount;
                var msgBossKill = "";
                if (bossKills > 0) {
                    msgBossKill = (kills == 1) ?
                        $" and {bossKills} boss kills" :
                        $" and {bossKills} boss kill";
                }
                killReport.AppendLine(msgKill + msgBossKill);
            }

            // Guest Stats
            // kills
            var guestKillStat = playerKillStats[playerKillStats.Count - 1];
            var guestMsgKill = (guestKillStat.DieCount == 1) ?
                $"And our {guestKillStat.PlayerEmote} have {guestKillStat.DieCount} kill" :
                $"And our {guestKillStat.PlayerEmote} have {guestKillStat.DieCount} kills";
            // boss kills
            var guestBossKillStat = playerBossKillStats[playerBossKillStats.Count - 1];
            var guestMsgBossKill = "";
            if (guestBossKillStat.DieCount > 0) {
                guestMsgBossKill = (guestBossKillStat.DieCount == 1) ?
                    $" and {guestBossKillStat.DieCount} boss kill" :
                    $" and {guestBossKillStat.DieCount} boss kills";
            }
            killReport.AppendLine(guestMsgKill + guestMsgBossKill);

            // Final stats
            return killReport.ToString();
        }

        /// <summary>
        /// Concatenates a message containing the nat1 stat report
        /// </summary>
        /// <returns></returns>
        public string GetDownedReport()
        {
            List<PlayerStat> playerStats = CollectStats(StatOptions.Downed.ToString());
            StringBuilder downedReport = new StringBuilder();

            var total = GetStatTotal(playerStats);
            var totalMsg = (total == 1) ?
                $"Players have gone down a total of {total} time to date" :
                $"Players have gone down a total of {total} times to date";
            downedReport.AppendLine(totalMsg);

            // Order player list in descending order (exclude DM and Guest)
            var pcStats = playerStats.Skip(1).Take(playerStats.Count - 2).ToList();
            var orderedPCStats = from ps in pcStats
                                 orderby ps.DieCount descending
                                 select ps;
            // Player stats
            downedReport.AppendLine("Number of times downed per player are as follows:");
            foreach (PlayerStat pc in orderedPCStats) {
                var msg = (pc.DieCount == 1) ?
                    $"{pc.PlayerEmote} with {pc.DieCount} time knocked down" :
                    $"{pc.PlayerEmote} with {pc.DieCount} times knocked down";
                downedReport.AppendLine(msg);
            }

            // Guest Stats
            var guestStat = playerStats[playerStats.Count - 1];
            var guestMsg = (guestStat.DieCount == 1) ?
                $"And our {guestStat.PlayerEmote} have been knocked down {guestStat.DieCount} time" :
                $"And our {guestStat.PlayerEmote} have been knocked down {guestStat.DieCount} timess";
            downedReport.AppendLine(guestMsg);
            // Final stats
            return downedReport.ToString();
        }

        /// <summary>
        /// Gets stats for every player, including DM and Guests for a certain stat
        /// </summary>
        /// <returns></returns>
        private List<PlayerStat> CollectStats(string stat)
        {
            List<PlayerStat> playerStats = new List<PlayerStat>();

            foreach (NameOptions name in Enum.GetValues(typeof(NameOptions))) {
                playerStats.Add(new PlayerStat(name.ToString(), stat, sessionLog));
            }

            return playerStats;
        }

        /// <summary>
        /// Calculates the total number of a stat
        /// </summary>
        /// <param name="playerStats"></param>
        /// <returns></returns>
        private int GetStatTotal(List<PlayerStat> playerStats)
        {
            int total = 0;
            foreach (PlayerStat playerStat in playerStats) {
                total += playerStat.DieCount;
            }
            return total;
        }
    }
}
