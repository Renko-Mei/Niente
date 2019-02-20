using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Niente.Models
{
    public class ArticleDocument : INienteContent
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [MaxLength(100)]
        public string Subtitle { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime LastEditDate { get; set; }

        public string PreviewImageUri { get; set; }

        [MaxLength(50)]
        public string[] ImageUris { get; set; }
    }
}
