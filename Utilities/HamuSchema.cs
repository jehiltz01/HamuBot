using Newtonsoft.Json.Schema;

namespace HamuBot.Utilities
{
    /// <summary>
    /// Contains json schemas for sessionLog.json and HamuProverbs.json
    /// Used to validate json files input via discord to update respective files in the bot
    /// </summary>
    public class HamuSchemas
    {
        readonly JSchema critSchema;
        readonly JSchema combatSchema;

        /// <summary>
        /// Constructor
        /// Also initializes critSchema and combatSchema with the required lists of player names and ints for stats
        /// </summary>
        public HamuSchemas()
        {
            critSchema = new JSchema {
                Type = JSchemaType.Object,
                Properties =
                {
                    { "DM", new JSchema{ Type = JSchemaType.Integer } },
                    { "Amaya", new JSchema{ Type = JSchemaType.Integer } },
                    { "Cal", new JSchema{ Type = JSchemaType.Integer } },
                    { "Calem", new JSchema{ Type = JSchemaType.Integer } },
                    { "Rayna", new JSchema{ Type = JSchemaType.Integer } },
                    { "Reggie", new JSchema{ Type = JSchemaType.Integer } },
                    { "Trix", new JSchema{ Type = JSchemaType.Integer } },
                    { "Guests", new JSchema{ Type = JSchemaType.Integer } },
                },
                Required = {
                    "DM", "Amaya", "Cal", "Calem", "Rayna", "Reggie", "Trix", "Guests"
                }
            };

            combatSchema = new JSchema {
                Type = JSchemaType.Object,
                Properties =
                {
                    { "Amaya", new JSchema{ Type = JSchemaType.Integer } },
                    { "Cal", new JSchema{ Type = JSchemaType.Integer } },
                    { "Calem", new JSchema{ Type = JSchemaType.Integer } },
                    { "Rayna", new JSchema{ Type = JSchemaType.Integer } },
                    { "Reggie", new JSchema{ Type = JSchemaType.Integer } },
                    { "Trix", new JSchema{ Type = JSchemaType.Integer } },
                    { "Guests", new JSchema{ Type = JSchemaType.Integer } },
                },
                Required = {
                    "Amaya", "Cal", "Calem", "Rayna", "Reggie", "Trix", "Guests"
                }
            };
        }

        /// <summary>
        /// Schema for sessionLog.json
        /// </summary>
        /// <returns></returns>
        public JSchema GetSessionLogSchema()
        {
            JSchema slSchema = new JSchema {
                Type = JSchemaType.Object,
                Properties =
                {
                    { "month", new JSchema { Type = JSchemaType.String} },
                    { "day", new JSchema { Type = JSchemaType.String} },
                    { "monthly", new JSchema { Type = JSchemaType.String} },
                    { "countdownName", new JSchema { Type = JSchemaType.String} },
                    { "countdownNumber", new JSchema { Type = JSchemaType.String} },
                    { "reminder", new JSchema { Type = JSchemaType.String} },
                    { "customOpening", new JSchema { Type = JSchemaType.String} },
                    { "customSignoff", new JSchema { Type = JSchemaType.String} },
                    { "nat20s", critSchema },
                    { "nat1s", critSchema },
                    { "kills", combatSchema },
                    { "bossKills", combatSchema },
                    { "downed", combatSchema }
                },
                Required = {
                    "month", "day", "monthly", "countdownName", "countdownNumber", "reminder", "customOpening", "customSignoff", "nat20s", "nat1s", "kills", "bossKills", "downed"
                }
            };
            return slSchema;
        }

        /// <summary>
        /// Schema for HamuProverbs.json
        /// </summary>
        /// <returns></returns>
        public JSchema GetHamuProverbsSchema()
        {
            JSchema hpSchema = new JSchema {
                Type = JSchemaType.Object,
                Properties =
                {
                    { "proverbs", new JSchema
                    {
                            Type = JSchemaType.Array,
                            Items = { new JSchema { Type = JSchemaType.String } }
                        }
                    }
                },
                Required = {
                    "proverbs"
                }
            };
            return hpSchema;
        }
    }
}
