using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Niente.Models
{
    public interface INienteContent
    {
        int Id { get; set; }
        string Body { get; set; }
        DateTime CreateAt { get; set; }
    }

    public enum DisplayLevel
    {
        Default,
        Admin_Only
    }

    public enum Language
    {
        Chinese,
        English,
        Universal
    }

    public enum Status
    {
        Visible,
        Hidden
    }
}
