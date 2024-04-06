using HamuBot.Extensions;
using HamuBot.Utilities;
using System.Text;

namespace HamuBot.Calendar
{
    /// <summary>
    /// CimmerianCalendar object
    /// </summary>
    public class CimmerianCalendar
    {
        private readonly List<CimmerianDate> ListOfDates;
        private CimmerianDate curDate;

        public CimmerianCalendar()
        {
            ListOfDates = new List<CimmerianDate>();
            try {
                // Read calendar file 
                string[] calendarContents = File.ReadAllLines("CimmerianCalendar.csv");
                // Remove headers
                string[] calendarData = calendarContents.Skip(1).ToArray();
                // Parse file
                ParseCalendarData(calendarData);
            } catch (Exception ex) {
                Console.WriteLine("Error processing CimmerianCalendar.csv");
            }
        }

        /// <summary>
        /// Gets the current date, ready to be printed
        /// </summary>
        /// <returns></returns>
        public string GetCurrentDate()
        {
            return curDate.GetDateToPrint();
        }

        /// <summary>
        /// Sets the current date to the date input with the month and day
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public void SetDate(int month, int day)
        {
            curDate = GetDate(month, day);
            UpdateDateInSessionLog();
        }

        /// <summary>
        /// Increments the date by a number of days equal to count
        /// </summary>
        /// <param name="count"></param>
        /// <param name="forecast"> true if only want to forecast, false will actually update the current date </param>
        /// <param name="printSimple"> true if the date to be printed should be in a simplified format; really only used for countdown setting
        /// <returns> A string with the message printing out a new date and any potentially missed events OR a forecast of that date </returns>
        public string IncrementDate(int count, bool forecast, bool printSimple)
        {
            if (count <= 0) {
                return "The amount incremented by must be greater than 0";
            }

            var curIndex = ((curDate.Month - 1) * 28 + curDate.Day) - 1;
            curIndex++; // need to add a day so as to not count the current/original day 
            // Iterate over list, looping back around and storing the missed over dates into a new list
            var missedDates = ListOfDates.Iterate(curIndex, count).ToList<CimmerianDate>();

            // Countdown calculation
            var sessionLog = GetSessionLog();
            if (sessionLog.CountdownName != null && sessionLog.CountdownName != "") {
                var daysRemaining = Int32.Parse(sessionLog.CountdownNumber) - missedDates.Count;
                sessionLog.CountdownNumber = daysRemaining.ToString();
                UpdateSessionLog(sessionLog);
            }

            // Count how many months changed (if any)
            var monthsPassed = 0;
            var curMon = curDate.Month;
            for (int i = 0; i < missedDates.Count; i++) {
                if (curMon != missedDates[i].Month) {
                    curMon = missedDates[i].Month;
                    monthsPassed++;
                }
            }

            // Get new date and then remove it from missed dates list so it is not counted twice
            var newDate = missedDates[missedDates.Count - 1];
            missedDates.RemoveAt(missedDates.Count - 1);

            // If its just a forecast, get that message and return
            if (forecast) {
                if (printSimple) {
                    return newDate.GetSimpleDateToPrint();
                }
                return newDate.GetForecastedDateToPrint();
            }

            // Otherwise, update current date and update sessionLog.json
            curDate = newDate;
            UpdateDateInSessionLog();

            // Check if its the new year
            var newYear = false;
            if (newDate.Month == 1 && curDate.Month != 1) {
                newYear = true;
            }

            StringBuilder dateMsg = new StringBuilder();

            dateMsg.AppendLine(newDate.GetDateToPrint());

            // Check for missed events if incremented by more than 1 day
            if (count > 1) {
                dateMsg.AppendLine("~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~");
                // Monthly reminders if incremented by over a month
                if (monthsPassed == 1) {
                    dateMsg.AppendLine($"{sessionLog.Monthly}.");
                } else {
                    dateMsg.AppendLine($"{sessionLog.Monthly} {monthsPassed} times.");
                }

                var birthdays = new List<string>();
                var holidays = new List<string>();
                foreach (CimmerianDate missedDate in missedDates) {
                    if (missedDate.Birthday != null && missedDate.Birthday != "") {
                        birthdays.Add(missedDate.Birthday);
                    }
                    if (missedDate.Holiday != null && missedDate.Holiday != "") {
                        holidays.Add(missedDate.Holiday);
                    }
                }
                if (birthdays.Count > 0) {
                    StringBuilder bdaysMsg = new StringBuilder();
                    bdaysMsg.Append($"Happy Belated Birthday to ");
                    for (int i = 0; i < birthdays.Count; i++) {
                        if (i > 0 && birthdays.Count > 2) {
                            bdaysMsg.Append(", ");
                        }
                        if (i == birthdays.Count - 1 && birthdays.Count > 1) {
                            bdaysMsg.Append("and ");
                        }
                        bdaysMsg.Append($"{birthdays[i]}");
                    }
                    bdaysMsg.Append("!!! :partying_face::birthday:");
                    dateMsg.AppendLine(bdaysMsg.ToString());
                }
                if (holidays.Count > 0) {
                    StringBuilder holidaysMsg = new StringBuilder();
                    holidaysMsg.Append($"Over this break, Cimmerians celebrated ");
                    for (int i = 0; i < holidays.Count; i++) {
                        if (i > 0 && holidays.Count > 2) {
                            holidaysMsg.Append(", ");
                        }
                        if (i == holidays.Count - 1 && holidays.Count > 1) {
                            holidaysMsg.Append("and ");
                        }
                        holidaysMsg.Append($"{holidays[i]}");
                    }
                    dateMsg.AppendLine(holidaysMsg.ToString());
                }
            }
            if (newYear) {
                dateMsg.AppendLine("~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~");
                dateMsg.AppendLine("Happy New Year! :fireworks:");
            }

                return dateMsg.ToString();
        }

        /// <summary>
        /// Generates a random date that is different from the current date
        /// Returns the date in the simplified string format
        /// </summary>
        /// <returns></returns>
        public string GetRandomDate()
        {
            var randDate = curDate;

            while (randDate == curDate) {
                var randIndex = new Random().Next(336);
                if (!ListOfDates[randIndex].Equals(curDate)) {
                    randDate = ListOfDates[randIndex];
                }
            }
            return randDate.GetSimpleDateToPrint();
        }

        /// <summary>
        /// Gets a date from a month and a day
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private CimmerianDate GetDate(int month, int day)
        {
            // Index in the ListOfDates list of the input date
            var dateIndex = ((month - 1) * 28 + day) - 1;
            var desiredDate = ListOfDates[dateIndex];
            if (desiredDate == null || desiredDate.Month != month || desiredDate.Day != day) {
                throw new InvalidOperationException("Error getting date; dates do not match");
            }
            return desiredDate;
        }

        /// <summary>
        /// Parses calendar data and creates the ListOfDates from it
        /// </summary>
        /// <param name="calendarData"></param>
        private void ParseCalendarData(string[] calendarData)
        {
            foreach (string row in calendarData) {
                string[] column = row.Split(',');
                CimmerianDate date = new CimmerianDate(Int32.Parse(column[0]), column[1], Int32.Parse(column[2]), column[3], column[4], column[5]);
                ListOfDates.Add(date);
            }
            GetCurDateFromSessionLog();
        }

        /// <summary>
        /// Gets the current date from teh session log
        /// </summary>
        private void GetCurDateFromSessionLog()
        {
            var sessionLog = GetSessionLog();
            int curMonth = Int32.Parse(sessionLog.Month);
            int curDay = Int32.Parse(sessionLog.Day);
            curDate = GetDate(curMonth, curDay);
        }

        /// <summary>
        /// Updates the sessionLog.json with the new current date
        /// </summary>
        private void UpdateDateInSessionLog()
        {
            var sessionLog = GetSessionLog();
            sessionLog.Month = curDate.Month.ToString();
            sessionLog.Day = curDate.Day.ToString();
            UpdateSessionLog(sessionLog);
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
