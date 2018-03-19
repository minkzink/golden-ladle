using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenLadle.Models
{
    public class Event : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Start Date/Time")]
        public DateTime StartDT { get; set; }
        [Required]
        [Display(Name = "End Date/Time")]
        public DateTime EndDT { get; set; }
        
        public List<Entry> Entries { get; set; }
        public ICollection<FilePath> FilePaths { get; set; }
    }
}
