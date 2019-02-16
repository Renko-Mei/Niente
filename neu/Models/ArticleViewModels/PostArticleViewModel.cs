using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace neu.Models.ArticleViewModels
{
    public class PostArticleViewModel
    {
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [MaxLength(100)]
        public string Subtitle { get; set; }
    }
}
