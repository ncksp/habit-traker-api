using Abc.HabitTracker.Api.Tracker.Constants;
using Abc.HabitTracker.Api.Tracker.Database;
using Abc.HabitTracker.Api.Tracker.Response;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Abc.HabitTracker.Api.Tracker.Habit
{
    public class Log
    {
        public Log(Int16 currentStreak, Int16 longestStreak, Int16 logCount, DateTime[] logs)
        {
            this.CurrentStreak = currentStreak;
            this.LongestStreak = longestStreak;
            this.LogCount = logCount;
            this.Logs = HabitLogs.SetLogs(logs);
        }
        public Int16 CurrentStreak { get; private set; }
        
        public Int16 LongestStreak { get; private set; }
        
        public Int16 LogCount { get; private set; }

        public List<DateTime> Logs { get; private set; }
    }

    public class HabitLogs
    {
        private static List<DateTime> logList;

        public static List<DateTime> SetLogs(DateTime[] logs)
        {
            logList = new List<DateTime>();
            foreach (DateTime time in logs)
            {
                logList.Add(time);
            }
            return logList;
        }

        public static HabitResponse AddLog(Guid userID, Guid id)
        {
            NpgsqlConnection connection = Connection.GetConnection();
            NpgsqlTransaction transaction = connection.BeginTransaction();
            IHabitRepository repository = new HabitRepository(connection, transaction);

            var get = repository.Find(userID, id);
            if (get == null)
                return null;

            repository.AddLog(userID, id);

            get = repository.Find(userID, id);

            return get;
        }

    }
}
