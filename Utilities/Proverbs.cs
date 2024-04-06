using Newtonsoft.Json;

namespace HamuBot.Utilities
{
    public class HamuProverbs
    {
        [JsonProperty("proverbs")]
        public string[]? Proverbs { get; set; }

        [JsonProperty("openers")]
        public string[]? Openers { get; set; }

    }
}
