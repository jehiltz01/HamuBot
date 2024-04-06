namespace HamuBot.Utilities
{
    public class PlayerStat
    {
        public int DieCount { get; set; }
        public string PlayerName { get; set; }

        public string PlayerEmote { get; set; }

        private string Stat;

        private SessionTracker SessionLog;

        private EmoteManager EmoteManager;

        /// <summary>
        /// Contains information for a player's stat 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stat"></param>
        /// <param name="sessionLog"></param>
        public PlayerStat(string name, string stat, SessionTracker sessionLog)
        {
            PlayerName = name;
            Stat = stat;
            SessionLog = sessionLog;
            DieCount = SetPlayerStat();
            EmoteManager = new EmoteManager();
            PlayerEmote = EmoteManager.GetEmote(name);
        }

        /// <summary>
        /// Sets a players stat (DieCount) based on the stat
        /// </summary>
        /// <returns></returns>
        private int SetPlayerStat()
        {
            // Gets sessionLog[stat]
            var statRef = SessionLog.GetType().GetProperty(Stat).GetValue(SessionLog, null);
            // Gets sessionLog[stat][PlayerName]
            DieCount = Convert.ToInt32(statRef.GetType().GetProperty(PlayerName).GetValue(statRef, null));

            return DieCount;
        }
    }
}
