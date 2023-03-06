using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models
{
    public class TodoQueryWM
    {
        public List<int>? TodoIds { get; set; }
        public string? Title { get; set; }
    }
}
