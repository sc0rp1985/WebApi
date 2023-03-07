using Common;
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

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (TodoIds.IsNullOrEmpty())
                sb.AppendLine("TodoIds - {}");
            else
            {
                sb.Append("TodoIds - {");
                sb.Append(string.Join(",", TodoIds.Select(x => x.ToString())));
                sb.Append("}");
            }
            sb.AppendLine($"Title - {Title}");
            return sb.ToString();
        }
    }
}
