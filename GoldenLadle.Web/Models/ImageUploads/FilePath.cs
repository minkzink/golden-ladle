using System.ComponentModel.DataAnnotations;

namespace GoldenLadle.Models
{
    public class FilePath
    {
        public int FilePathId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}
