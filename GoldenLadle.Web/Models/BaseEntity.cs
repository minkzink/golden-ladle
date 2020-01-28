using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace GoldenLadle.Models
{
    public class BaseEntity
    {
        [Required]
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
