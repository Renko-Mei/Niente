using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace neu.Models
{
    public class Article : INeuContent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [MaxLength(100)]
        public string Subtitle { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastEditDate { get; set; }
    }
}
