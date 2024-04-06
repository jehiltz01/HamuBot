namespace HamuBot.Utilities
{
    public class EmoteManager
    {
        public EmoteManager() { }

        public string GetEmote(string emoteName)
        {
            string emoteId = ":sweat_smile:";

            switch (emoteName) {
                case ("DM"):
                    emoteId = "<:DM:826836329525936189>";
                    break;
                case ("Amaya"):
                    emoteId = "<:Amaya:804349623313498162>";
                    break;
                case ("Cal"):
                    emoteId = "<:Cal:801513898468573215>";
                    break;
                case ("Calem"):
                    emoteId = "<:Calem:802575174443597835>";
                    break;
                case ("Rayna"):
                    emoteId = "<:Rayna:801514411784798228>";
                    break;
                case ("Reggie"):
                    emoteId = "<:Reggie:801592356024746004>";
                    break;
                case ("Trix"):
                    emoteId = "<:Trix:803464882417958952>";
                    break;
                case ("Guests"):
                    emoteId = "<:Guests:894071756359352340>";
                    break;
                case ("Nat20"):
                    emoteId = "<:Nat20:802584425153953862>";
                    break;
                case ("Nat1"):
                    emoteId = "<:Nat1:802584175777415198>";
                    break;
                case ("Eclipse"):
                    emoteId = "<:EdgyEclipse:748713983355125811>";
                    break;
            }

            return emoteId;
        }
    }
}
