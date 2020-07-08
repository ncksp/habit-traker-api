using Abc.HabitTracker.Api.Tracker.Habit;
using Abc.HabitTracker.Api.Tracker.Response;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abc.HabitTracker.Api.Tracker.Database
{
    public class HabitRepository : IHabitRepository
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;

        public HabitRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        public void Create(Habit.Habit habit)
        {
            string query = "INSERT INTO habits (id, name, day_off, user_id, created_at) VALUES (@id, @name, @day_off, @user_id, @created_at)";
            using var cmd = new NpgsqlCommand(query, _connection, _transaction);
            cmd.Parameters.AddWithValue("id", Guid.NewGuid());
            cmd.Parameters.AddWithValue("name", habit.Name.Value);
            cmd.Parameters.AddWithValue("day_off", habit.DaysOff.Value);
            cmd.Parameters.AddWithValue("user_id", habit.UserID);
            cmd.Parameters.AddWithValue("created_at", habit.CreatedAt);

            cmd.ExecuteNonQuery();
        }

        public void Delete(Guid id, Guid userId)
        {
            string query = "DELETE FROM habits WHERE id = @id AND user_id = @user_id";
            using var cmd = new NpgsqlCommand(query, _connection, _transaction);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("user_id", userId);

            cmd.ExecuteNonQuery();
        }

        public HabitResponse Find(Guid userId, Guid id)
        {
            string query = "select * from habits where user_id = @user_id AND id = @id";

            HabitResponse data;
            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("user_id", userId);
                cmd.Parameters.AddWithValue("id", id);
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                if(!reader.Read())
                    return null;

                data = new HabitResponse(
                    new Habit.Habit(
                        reader.GetGuid(0),
                        reader.GetString(1),
                        (String[])reader.GetValue(2),
                        reader.GetInt16(3),
                        reader.GetInt16(4),
                        reader.GetInt16(5),
                        (DateTime[])reader.GetValue(6),
                        reader.GetGuid(7),
                        reader.GetDateTime(8)
                    )
                );
            }

            return data;
        }

        public List<HabitResponse> GetHabits(Guid userId)
        {
            string query = "select * from habits where user_id = @user_id";

            List<HabitResponse> data = new List<HabitResponse>();
            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("user_id", userId);
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(new HabitResponse(
                        new Habit.Habit(
                            reader.GetGuid(0),
                            reader.GetString(1),
                            (String[])reader.GetValue(2),
                            reader.GetInt16(3),
                            reader.GetInt16(4),
                            reader.GetInt16(5),
                            (DateTime[])reader.GetValue(6),
                            reader.GetGuid(7),
                            reader.GetDateTime(8)
                        )
                    ));
                }
            }
            return data;
        }

        public void Update(Guid id, RequestData request, Guid userId)
        {
            string query = "UPDATE habits SET name = @name, day_off = @day_off WHERE id = @id AND user_id = @user_id";
            using var cmd = new NpgsqlCommand(query, _connection, _transaction);
            cmd.Parameters.AddWithValue("name", request.Name);
            cmd.Parameters.AddWithValue("day_off", request.DaysOff);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("user_id", userId);

            cmd.ExecuteNonQuery();
        }

        public void AddLog(Guid userId, Guid id)
        {
            var data = Find(userId, id);
            DateTime[] newLogs = data.Logs.Append(DateTime.Now).Cast<DateTime>().ToArray();
            Int16 streak = data.CurrentStreak;
            Int16 longestStreak = data.LongestStreak;
            
            try
            {
                if (!FindStreakSnapshot(userId, id))
                    CreateStreakShot(userId, id);
                if (longestStreak == streak) 
                    longestStreak++;
                else 
                    longestStreak = GetLongestStreak(userId, id);

                string query = "UPDATE habits SET log_count = log_count + 1, logs = @logs, current_streak = current_streak + 1, longest_streak = @longest_streak WHERE id = @id AND user_id = @user_id";
                using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
                {
                    cmd.Parameters.AddWithValue("logs", newLogs);
                    cmd.Parameters.AddWithValue("user_id", userId);
                    cmd.Parameters.AddWithValue("longest_streak", longestStreak);
                    cmd.Parameters.AddWithValue("id", id);

                    cmd.ExecuteNonQuery();
                }

                AddStreakSnapshot(userId, id, streak + 1);
                _transaction.Commit();
            }
            catch (Exception e)
            {
                _transaction.Rollback();
                Console.WriteLine(e);
            }
        }

        private void AddStreakSnapshot(Guid userId, Guid habitId, int v)
        {
            string query = "INSERT INTO streak_snapshot (current_streak, user_id, habit_id) VALUES (@current_streak, @user_id, @habit_id)";
            using var cmd = new NpgsqlCommand(query, _connection, _transaction);
            cmd.Parameters.AddWithValue("user_id", userId);
            cmd.Parameters.AddWithValue("current_streak", v);
            cmd.Parameters.AddWithValue("habit_id", habitId);

            cmd.ExecuteNonQuery();
        }

        private Int16 GetLongestStreak(Guid userID, Guid habitID)
        {
            string query = "select MAX(current_streak) from streak_snapshot where user_id = @user_id AND habit_id = @habit_id";
            Int16 s = 0;
            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("user_id", userID);
                cmd.Parameters.AddWithValue("habit_id", habitID);
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                    s = 0;

                s = reader.GetInt16(0);
            }

            return s;
        }

        private void CreateStreakShot(Guid userId, Guid habitId)
        {
            string query = "INSERT INTO streak_snapshot (current_streak, user_id, habit_id) VALUES (0, @user_id, @habit_id)";
            using var cmd = new NpgsqlCommand(query, _connection, _transaction);
            cmd.Parameters.AddWithValue("user_id", userId);
            cmd.Parameters.AddWithValue("habit_id", habitId);

            cmd.ExecuteNonQuery();
        }

        private bool FindStreakSnapshot(Guid userID, Guid habitID)
        {
            string query = "select * from streak_snapshot where user_id = @user_id AND habit_id = @habit_id";

            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("user_id", userID);
                cmd.Parameters.AddWithValue("habit_id", habitID);
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                    return false;
            }

            return true;
        }
    }
}
