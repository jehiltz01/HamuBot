using HamuBot.Enums;
using HamuBot.Utilities;
using System.Text;

namespace HamuBot.Calendar
{
    /// <summary>
    /// Cimmerian Date Object
    /// Contains date information as well as functions to print the date
    /// </summary>
    public class CimmerianDate
    {
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int Day { get; set; }
        public string Weekday { get; set; }
        public string Birthday { get; set; }
        public string Holiday { get; set; }

        private EmoteManager emoteManager;

        public CimmerianDate(int Month, string MonthName, int Day, string Weekday, string Birthday, string Holiday)
        {
            this.Month = Month;
            this.MonthName = MonthName;
            this.Day = Day;
            this.Weekday = Weekday;
            this.Birthday = Birthday;
            this.Holiday = Holiday;
            emoteManager = new EmoteManager();
        }

        /// <summary>
        /// Builds a string for a simplified date
        /// Only outputs the date itself containing the weekday, the day, and the month
        /// </summary>
        /// <returns></returns>
        public string GetSimpleDateToPrint()
        {
            // Find Date suffix 
            string dateSuffix = "";
            var lastDigit = Day % 10;
            if (lastDigit > 3 || lastDigit < 1 || Day == 11 || Day == 12 || Day == 13) {
                dateSuffix = "th";
            } else if (lastDigit == 3) {
                dateSuffix = "rd";
            } else if (lastDigit == 2) {
                dateSuffix = "nd";
            } else {
                dateSuffix = "st";
            }
            return $"{Weekday} the {Day}{dateSuffix} of {MonthName}";
        }

        /// <summary>
        /// Prepares a CimmerianDate for printing in a desired format 
        /// </summary>
        /// <returns></returns>
        public string GetDateToPrint()
        {
            // Start building date message
            StringBuilder date = new StringBuilder();
            date.AppendLine(GetSimpleDateToPrint());

            // Birthday
            if (Birthday != null && Birthday != "") {
                // Check if its a party member 
                if (Enum.IsDefined(typeof(NameOptions), Birthday)) {
                    date.AppendLine($"Happy Birthday {Birthday}!!! {emoteManager.GetEmote(Birthday)} :birthday:");
                } else {
                    date.AppendLine($"Happy Birthday {Birthday}!!! :partying_face::birthday:");
                }
            }
            // Holiday
            if (Holiday != null && Holiday != "") {
                if (Holiday == "Festival of Love") {
                    date.AppendLine($"Today Cimmerians celebrate the Festival of Love :two_hearts:");
                } else if (Holiday == "Festival of the Arts") {
                    date.AppendLine($"Today Cimmerians celebrate the Festival of the Arts :art: :performing_arts: :notes:");
                } else if (Holiday == "Festival of Lights") {
                    date.AppendLine($"Today Cimmerians celebrate the Festival of Lights :sparkler:");
                } else {
                    date.AppendLine($"Today Cimmerians celebrate {Holiday}");
                }
            }

            //Countdown
            var sessionLog = GetSessionLog();
            if (sessionLog.CountdownName != null && sessionLog.CountdownName != "") {
                var countdownVal = Int32.Parse(sessionLog.CountdownNumber);
                // If countdown is still active, just alert how many days left
                if (countdownVal > 0) {
                    if (countdownVal == 1) {
                        date.AppendLine($"Be aware, there is {sessionLog.CountdownNumber} day left until {sessionLog.CountdownName}");
                    } else {
                        date.AppendLine($"Be aware, there are {sessionLog.CountdownNumber} days left until {sessionLog.CountdownName}");
                    }
                    // Otherwise, it is either the day of or past the date, so alert acordingly and delete the countdown
                } else {
                    if (countdownVal == 0) {
                        date.AppendLine($"Today, the time is up for {sessionLog.CountdownName}");
                    } else {
                        if (countdownVal == -1) {
                            date.AppendLine($"The deadline for {sessionLog.CountdownName} expired {Math.Abs(countdownVal)} day ago");
                        } else {
                            date.AppendLine($"The deadline for {sessionLog.CountdownName} expired {Math.Abs(countdownVal)} days ago");
                        }
                    }
                    sessionLog.CountdownName = "";
                    sessionLog.CountdownNumber = "";
                    UpdateSessionLog(sessionLog);
                }
            }
            return date.ToString();
        }

        /// <summary>
        /// Prepares a CimmerianDate for printing in a desired format 
        /// </summary>
        /// <returns></returns>
        public string GetForecastedDateToPrint()
        {
            // Start building date message
            StringBuilder date = new StringBuilder();
            date.AppendLine(GetSimpleDateToPrint());

            StringBuilder events = new StringBuilder();
            // Birthday
            if (Birthday != null && Birthday != "") {
                events.AppendLine($"{Birthday}'s birthday");
            }
            // Holiday
            if (Holiday != null && Holiday != "") {
                events.AppendLine($"{Holiday}");
            }
            if (events.Length > 0) {
                date.AppendLine("On that day will be:");
                date.AppendLine(events.ToString());
            }
            return date.ToString();
        }

        /// <summary>
        /// Custom Equals method
        /// Returns true if both dates have the same month and day
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CimmerianDate other)
        {
            if (Month == other.Month && Day == other.Day) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the session log 
        /// </summary>
        /// <returns></returns>
        private SessionTracker GetSessionLog()
        {
            var sessionLog = new SessionTracker();
            try {
                var outputDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                string slJson = File.ReadAllText(outputDir + "/sessionLog.json");
                sessionLog = Newtonsoft.Json.JsonConvert.DeserializeObject<SessionTracker>(slJson);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return sessionLog;
        }

        /// <summary>
        /// Updates the session log
        /// </summary>
        /// <param name="sessionLog"></param>
        private void UpdateSessionLog(SessionTracker sessionLog)
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
