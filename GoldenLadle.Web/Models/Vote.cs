using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenLadle.Models
{
    public class Vote : BaseEntity
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Range(1,3)]
        [JsonProperty(PropertyName = "value")]
        public byte Value { get; set; }

        [JsonProperty(PropertyName = "entryId")]
        public int EntryId { get; set; }
        [JsonIgnore]
        public Entry Entry { get; set; }

        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }
        [JsonIgnore]
        public Event Event { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
