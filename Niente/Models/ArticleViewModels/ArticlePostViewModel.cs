using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Niente.Models.ArticleViewModels
{
    public class ArticlePostViewModel
    {
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [MaxLength(100)]
        public string PreviewText { get; set; }

        public string PreviewImageUri { get; set; }
    }
}
