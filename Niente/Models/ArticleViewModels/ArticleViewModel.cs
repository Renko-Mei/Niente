using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Niente.Models.ArticleViewModels
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string PreviewText { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime LastEditAt { get; set; }

        public string PreviewImageUri { get; set; }

        public string[] ImageUris { get; set; }
    }
}
