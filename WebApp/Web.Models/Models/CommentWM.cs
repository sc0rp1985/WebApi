using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Models
{
    public class CommentWM
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public int TodoId { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Id = {Id}");
            sb.AppendLine($"Text = {Text}");
            sb.AppendLine($"TodoId = {TodoId}");            
            return sb.ToString();
        }
    }
}
