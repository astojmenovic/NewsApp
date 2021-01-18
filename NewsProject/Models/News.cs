using NewsProject.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsProject.Models
{
    public class News
    {
        [Key]       
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        public AppUser Author { get; set; }
        public string AuthorId { get; set; }
        public DateTime CreatedTimestamp { get; set; } = DateTime.Now;
    }
}
