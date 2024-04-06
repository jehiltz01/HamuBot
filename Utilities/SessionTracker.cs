using Newtonsoft.Json;

namespace HamuBot.Utilities
{
    public class SessionTracker
    {
        [JsonProperty("month")]
        public string? Month { get; set; }

        [JsonProperty("day")]
        public string? Day { get; set; }

        [JsonProperty("monthly")]
        public string? Monthly { get; set; }

        [JsonProperty("countdownName")]
        public string? CountdownName { get; set; }

        [JsonProperty("countdownNumber")]
        public string? CountdownNumber { get; set; }

        [JsonProperty("reminder")]
        public string? Reminder { get; set; }

        [JsonProperty("customOpening")]
        public string? CustomOpening { get; set; }

        [JsonProperty("customSignoff")]
        public string? CustomSignoff { get; set; }

        [JsonProperty("nat20s")]
        public Nat20? Nat20s { get; set; }

        [JsonProperty("nat1s")]
        public Nat1? Nat1s { get; set; }

        [JsonProperty("kills")]
        public Kill? Kills { get; set; }

        [JsonProperty("bossKills")]
        public BossKill? BossKills { get; set; }

        [JsonProperty("downed")]
        public Down? Downed { get; set; }

    }
    public class Nat20
    {
        [JsonProperty("DM")]
        public int DM { get; set; }

        [JsonProperty("Amaya")]
        public int Amaya { get; set; }

        [JsonProperty("Cal")]
        public int Cal { get; set; }

        [JsonProperty("Calem")]
        public int Calem { get; set; }

        [JsonProperty("Rayna")]
        public int Rayna { get; set; }

        [JsonProperty("Reggie")]
        public int Reggie { get; set; }

        [JsonProperty("Trix")]
        public int Trix { get; set; }

        [JsonProperty("Guests")]
        public int Guests { get; set; }
    }

    public class Nat1
    {
        [JsonProperty("DM")]
        public int DM { get; set; }

        [JsonProperty("Amaya")]
        public int Amaya { get; set; }

        [JsonProperty("Cal")]
        public int Cal { get; set; }

        [JsonProperty("Calem")]
        public int Calem { get; set; }

        [JsonProperty("Rayna")]
        public int Rayna { get; set; }

        [JsonProperty("Reggie")]
        public int Reggie { get; set; }

        [JsonProperty("Trix")]
        public int Trix { get; set; }

        [JsonProperty("Guests")]
        public int Guests { get; set; }
    }

    public class Kill
    {
        [JsonProperty("DM")]
        public int DM { get; set; }

        [JsonProperty("Amaya")]
        public int Amaya { get; set; }

        [JsonProperty("Cal")]
        public int Cal { get; set; }

        [JsonProperty("Calem")]
        public int Calem { get; set; }

        [JsonProperty("Rayna")]
        public int Rayna { get; set; }

        [JsonProperty("Reggie")]
        public int Reggie { get; set; }

        [JsonProperty("Trix")]
        public int Trix { get; set; }

        [JsonProperty("Guests")]
        public int Guests { get; set; }
    }

    public class BossKill
    {
        [JsonProperty("DM")]
        public int DM { get; set; }

        [JsonProperty("Amaya")]
        public int Amaya { get; set; }

        [JsonProperty("Cal")]
        public int Cal { get; set; }

        [JsonProperty("Calem")]
        public int Calem { get; set; }

        [JsonProperty("Rayna")]
        public int Rayna { get; set; }

        [JsonProperty("Reggie")]
        public int Reggie { get; set; }

        [JsonProperty("Trix")]
        public int Trix { get; set; }

        [JsonProperty("Guests")]
        public int Guests { get; set; }
    }

    public class Down
    {
        [JsonProperty("DM")]
        public int DM { get; set; }

        [JsonProperty("Amaya")]
        public int Amaya { get; set; }

        [JsonProperty("Cal")]
        public int Cal { get; set; }

        [JsonProperty("Calem")]
        public int Calem { get; set; }

        [JsonProperty("Rayna")]
        public int Rayna { get; set; }

        [JsonProperty("Reggie")]
        public int Reggie { get; set; }

        [JsonProperty("Trix")]
        public int Trix { get; set; }

        [JsonProperty("Guests")]
        public int Guests { get; set; }
    }
}
