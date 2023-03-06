using System.ComponentModel.DataAnnotations;
using System.Text;
using Web.Models.Attributes;

namespace Web.Models
{
    public class TodoWM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime? Created { get; set; }

        [Required(ErrorMessage = "обязательно для заполнения")]
        [CategoryValidation(ErrorMessage = "не разрешенное значение")]
        public string Category { get; set; }
        
        [Required(ErrorMessage = "обязательно для заполнения")]
        [ColorValidation(ErrorMessage = "не разрешенное значение")]
        public string Color { get; set; }

        public string Hash { get; set; }
        public List<CommentWM> Comments { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Id = {Id}");
            sb.AppendLine($"Title = {Title}");
            sb.AppendLine($"Created = {Created}");
            sb.AppendLine($"Category = {Category}");
            sb.AppendLine($"Color = {Color}");
            sb.AppendLine("Comments {");
            foreach (var comment in Comments)
                sb.AppendLine(comment.ToString());
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
