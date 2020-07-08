using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Abc.HabitTracker.Api.Tracker.Database;
using Npgsql;
using Abc.HabitTracker.Api.Tracker.Constants;
using Abc.HabitTracker.Api.Tracker.Response;
using Abc.HabitTracker.Api.Tracker.Service;

namespace Abc.HabitTracker.Api.Tracker.Habit
{
    public class RequestData
    {
        [JsonPropertyName("name")]
        public String Name { get; set; }

        [JsonPropertyName("days_off")]
        public String[] DaysOff { get; set; }
    }

    public class Name
    {
        public String Value { get; private set; } 
        public Name(string val)
        {
            if (val.Length < 2 || val.Length > 100)
                throw new Exception("Name length must be between 2 and 100 characters");

            this.Value = val;
        }
    }
    public class Habit
    {

        private static NpgsqlConnection connection = Connection.GetConnection();
        private static IHabitRepository repository = new HabitRepository(connection, null);
        public Guid ID { get; set; }
        
        public Name Name { get; set; }
        
        public DayOff DaysOff { get; set; }
        
        public Log Log { get; private set; }

        public Guid UserID { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public Habit(Guid Id, String name, String[] daysOff, Int16 currentStreak, Int16 longestStreak, Int16 logCount, DateTime[] logs, Guid userId, DateTime createdAt)
        {
            this.ID = Id;
            this.Name = new Name(name);
            this.DaysOff = DayOff.GetInstance().SetDayOff(daysOff);
            this.Log = new Log(currentStreak, longestStreak, logCount, logs);
            this.UserID = userId;
            this.CreatedAt = createdAt;
        }

        public Habit() { }

        public static Habit NewHabit(RequestData data, Guid userID)
        {
            if(data.DaysOff == null)
            {
                data.DaysOff = new string[1];
                data.DaysOff[0] = "";
            }
            var newData = HabitFactory.Create(data, userID);
            if (newData == null)
                return null;

            repository.Create(newData);
            return newData;
        }

        public static HabitResponse UpdateHabit(Guid id, RequestData data, Guid userID)
        {
            if (data.DaysOff == null)
            {
                data.DaysOff = new string[1];
                data.DaysOff[0] = "";
            }

            repository.Update(id, data, userID);
            var newData = repository.Find(userID, id);
            return newData;
        }

        public static HabitResponse DeleteHabit(Guid id, Guid userID)
        {
            var newData = repository.Find(userID, id);
            if (newData == null)
                return null;

            repository.Delete(id,userID);
            return newData;
        }

        public static List<HabitResponse> GetHabits(Guid userID)
        {
            return repository.GetHabits(userID);
        }

        public static HabitResponse GetHabit(Guid userId, Guid id)
        {
            return repository.Find(userId,id);
        }

        public static HabitResponse AddLog(Guid userID, Guid id)
        {
            var data = HabitLogs.AddLog(userID, id);

            if (data == null)
                return null;

            UserLogged.Notify(data);

            return data;
        }
    }
}
