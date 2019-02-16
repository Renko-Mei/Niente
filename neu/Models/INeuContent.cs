using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace neu.Models
{
    public interface INeuContent
    {
        int Id { get; set; }
        string Content { get; set; }
        DateTime CreateDate { get; set; }
    }
}
