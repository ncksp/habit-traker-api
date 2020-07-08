using Abc.HabitTracker.Api.Tracker.Habit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Abc.HabitTracker.Api.Tracker.Response
{
    public class HabitResponse
    {

        [JsonPropertyName("id")]
        public Guid ID { get; private set; }

        [JsonPropertyName("name")]
        public String Name { get; private set; }

        [JsonPropertyName("days_off")]
        public String[] DaysOff { get; private set; }

        [JsonPropertyName("current_streak")]
        public Int16 CurrentStreak { get; private set; }

        [JsonPropertyName("longest_streak")]
        public Int16 LongestStreak { get; private set; }

        [JsonPropertyName("log_count")]
        public Int16 LogCount { get; private set; }

        [JsonPropertyName("logs")]
        public IEnumerable<DateTime> Logs { get; private set; }

        [JsonPropertyName("user_id")]
        public Guid UserID { get; private set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; private set; }

        public HabitResponse(Habit.Habit habit)
        {
            this.ID = habit.ID;
            this.Name = habit.Name.Value;
            this.DaysOff = habit.DaysOff.Value;
            this.CurrentStreak = habit.Log.CurrentStreak;
            this.LongestStreak = habit.Log.LongestStreak;
            this.LogCount = habit.Log.LogCount;
            this.Logs = habit.Log.Logs;
            this.UserID = habit.UserID;
            this.CreatedAt = habit.CreatedAt;
        }
    }
}
