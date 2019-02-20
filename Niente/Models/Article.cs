using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Niente.Models
{
    public class Article : INienteContent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [MaxLength(100)]
        public string Subtitle { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastEditAt { get; set; }

        public string PreviewImageUri { get; set; }

        public Uri[] ImageUris { get; set; }
    }
}
