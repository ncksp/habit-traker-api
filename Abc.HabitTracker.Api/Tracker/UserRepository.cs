using Abc.HabitTracker.Api.Tracker.Constants;
using Npgsql;
using System;

namespace Abc.HabitTracker.Api.Tracker
{
    public class UserRepository
    {
        private static NpgsqlConnection _connection = Connection.GetConnection();
        public User Find(Guid userId)
        {
            string query = "select * from users where id = @user_id";

            User data = null;
            using (var cmd = new NpgsqlCommand(query, _connection, null))
            {
                cmd.Parameters.AddWithValue("user_id", userId);
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                    return data;

                data = new User(
                    reader.GetGuid(0),
                    reader.GetString(1)
                );
            }

            return data;
        }
    }
}