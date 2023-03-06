using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IGetTodoService
    {
        Task<List<TodoDto>> ListAsync(TodoQueryDto query);
        Task AddTodoAsync(TodoDto todo);
        Task<TodoDto> GetAsync(int todoId);
        Task DeleteTodoAsync(int todoId);
        Task UpdateTodo(TodoDto todo);
        Task <List<CommentDto>> Async(int todoId);
        Task AddCommentAsync(int todoId, CommentDto comment);
    }

    public class TodoDto 
    {
        public int Id { get; set; }        
        public string Title { get; set; }                
        public DateTime? Created { get; set; }        
        public string Category { get; set; }        
        public string Color { get; set; }
        //public string Hash { get; set; }
        public List<CommentDto> Comments { get; set; }
    }

    public class CommentDto 
    {
        public int Id { get; set; }        
        public string Text { get; set; }
        public int TodoId { get; set; }
    }

    public class TodoQueryDto
    { 
        public List<int> TodoIds { get; set; }
        public string Title { get; set; }
    }

}
