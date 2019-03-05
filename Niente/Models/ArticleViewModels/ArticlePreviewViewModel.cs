using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Niente.Models.ArticleViewModels
{
    public class ArticlePreviewViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PreviewText { get; set; }

        public DateTime CreateAt { get; set; }

        public string PreviewImageUri { get; set; }
    }
}
