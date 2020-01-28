using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoldenLadle.Models
{
    public class Vote : BaseEntity
    {
        public int Id { get; set; }

        [Range(1,3)]
        public byte Value { get; set; }

        public int EntryId { get; set; }
        [JsonIgnore]
        public Entry Entry { get; set; }

        public int EventId { get; set; }
        [JsonIgnore]
        public Event Event { get; set; }

        public string UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
