using Abc.HabitTracker.Api.Tracker.Badge;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abc.HabitTracker.Api.Tracker.Database
{
    public class BadgeRepository : IBadgeRepository
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;
        public BadgeRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }
        public void Create(Badge.Badge badge)
        {
            string query = "INSERT INTO badges VALUES (@id, @name, @desc, @user_id)";
            using var cmd = new NpgsqlCommand(query, _connection, _transaction);
            cmd.Parameters.AddWithValue("id", badge.ID);
            cmd.Parameters.AddWithValue("name", badge.Name);
            cmd.Parameters.AddWithValue("desc", badge.Description );
            cmd.Parameters.AddWithValue("user_id", badge.UserID);

            cmd.ExecuteNonQuery();
        }

        public List<Badge.Badge> Find(Guid userID)
        {
            string query = "select * from badges where user_id = @user_id";
            List<Badge.Badge> badges = new List<Badge.Badge>();
            Badge.Badge data;
            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("user_id", userID);
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    data = new Badge.Badge(reader.GetGuid(0), reader.GetString(1), reader.GetString(2), reader.GetGuid(3), reader.GetDateTime(4));

                    badges.Add(data);
                }
            }

            return badges;
        }

        public Badge.Badge GetBadge(Guid userID, string name)
        {
            string query = "select * from badges where user_id = @user_id AND name = @name";

            Badge.Badge data;
            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("user_id", userID);
                cmd.Parameters.AddWithValue("name", name);
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                    return null;

                data = new Badge.Badge(reader.GetGuid(0), reader.GetString(1), reader.GetString(2), reader.GetGuid(3), reader.GetDateTime(4));
            }

            return data;
        }

        public void AddWorkaholicSnapshot(Guid userID, DateTime date)
        {
            string query = "INSERT INTO workaholic_snapshot (date, user_id) VALUES (@date, @user_id)";
            using var cmd = new NpgsqlCommand(query, _connection, _transaction);
            cmd.Parameters.AddWithValue("date", date);
            cmd.Parameters.AddWithValue("user_id", userID);

            cmd.ExecuteNonQuery();
        }

        public Int64 CountWorkaholicSnapshot(Guid userID)
        {
            string query = "select count(date) from workaholic_snapshot where user_id = @user_id";
            using var cmd = new NpgsqlCommand(query, _connection, _transaction);
            cmd.Parameters.AddWithValue("user_id", userID);
            using NpgsqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
                return 0;

            return reader.GetInt64(0);
        }
    }
}
