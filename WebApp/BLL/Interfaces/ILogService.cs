using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ILogService
    {
        Task AddLog(LogDto dto);
    }

    public class LogDto 
    {
        public int Id { get; set; }
        public DateTime Logged { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public string? Parameters { get; set; }
        public string? Message { get; set; }
        public Guid Correlation { get; set; }
        public long Duration { get; set; }
        public int Status { get; set; }
    }

}
