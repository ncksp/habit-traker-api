using System;
using System.Text.Json.Serialization;

namespace Abc.HabitTracker.Api.Tracker
{
    public class User
    {
        [JsonPropertyName("user_id")]
        public Guid Id { private set; get; }
        
        [JsonPropertyName("user_name")]
        public String Name { private set; get; }

        public User(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}