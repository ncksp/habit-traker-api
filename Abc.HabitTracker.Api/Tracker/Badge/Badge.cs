using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Abc.HabitTracker.Api.Tracker;
using Npgsql;
using Abc.HabitTracker.Api.Tracker.Constants;
using Abc.HabitTracker.Api.Tracker.Database;

namespace Abc.HabitTracker.Api.Tracker.Badge
{
    public class Badge
    {
        private static readonly NpgsqlConnection connection = Connection.GetConnection();
        private static readonly IBadgeRepository repository = new BadgeRepository(connection, null);

        [JsonPropertyName("id")]
        public Guid ID { get; private set; }

        [JsonPropertyName("name")]
        public String Name { get; private set; }

        [JsonPropertyName("description")]
        public String Description { get; private set; }

        [JsonPropertyName("user_id")]
        public Guid UserID { get; private set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; private set; }

        public Badge(Guid iD, string name, string description, Guid userID, DateTime createdAt)
        {
            ID = iD;
            Name = name;
            Description = description;
            this.UserID = userID;
            CreatedAt = createdAt;
        }

        public static List<Badge> GetBadge(Guid userID)
        {
            return repository.Find(userID);
        }
    }
}
