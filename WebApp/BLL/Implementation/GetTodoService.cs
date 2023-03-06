using AutoMapper;
using Common;
using DAO;
using Microsoft.EntityFrameworkCore;

namespace BLL
{
    public class GetTodoService : IGetTodoService
    {
        private readonly IDataProvider _dataProvider;
        private readonly IMapper _mapper;

        public GetTodoService(IDataProvider dataProvider, IMapper mapper)
        {
            _dataProvider = dataProvider;
            _mapper = mapper;
        }

        async public Task AddCommentAsync(int todoId, CommentDto comment)
        {
            var daoTodo = await _dataProvider.Select<Todo>().FirstOrDefaultAsync(x => x.Id == todoId);
            if (daoTodo != null)
            {
                var daoComment = _mapper.Map<Comment>(comment);
                daoComment.TodoId = todoId;
                await _dataProvider.Insert<Comment>(daoComment);
                await _dataProvider.SaveAsync();
            }
        }

        async public Task AddTodoAsync(TodoDto todo)
        {
            todo.Created = DateTime.Now;
            var daoObj = _mapper.Map<Todo>(todo);
            await _dataProvider.Insert<Todo>(daoObj);
            await _dataProvider.SaveAsync();
        }

        async public Task DeleteTodoAsync(int todoId)
        {
            var daoTodo = await _dataProvider.Select<Todo>().Include(x=>x.Comments).FirstOrDefaultAsync(x => x.Id == todoId);
            if (daoTodo != null)
            {
                await _dataProvider.Delete<Todo>(daoTodo);
                await _dataProvider.SaveAsync();
            }
            
        }

        async public Task<TodoDto> GetAsync(int todoId)
        {
            var todo = await _dataProvider.Select<Todo>().Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == todoId);
            //return todo != null ?  _mapper.Map<TodoDto>(todo) : new TodoDto();
            return _mapper.Map<TodoDto>(todo);
        }

        async public Task<List<CommentDto>> Async(int todoId)
        {
            var comments = await _dataProvider.Select<Comment>().Where(x => x.TodoId == todoId).ToListAsync();
            var result = comments.Select(x => _mapper.Map<CommentDto>(x)).ToList();
            return result;
        }        

        async public Task<List<TodoDto>> ListAsync(TodoQueryDto query)
        {
            IQueryable<Todo> daoQuery = _dataProvider.Select<Todo>().Include(x => x.Comments);
            if (query?.Title.IsNullOrEmpty() == false)
                daoQuery = daoQuery.Where(x => x.Title.Contains(query.Title));
            if (query?.TodoIds.IsNullOrEmpty() == false)
                daoQuery = daoQuery.Where(x => query.TodoIds.Contains(x.Id));
            var daoTodos =await daoQuery.ToListAsync();
            var result = daoTodos.Select(x => _mapper.Map<TodoDto>(x)).ToList();
            return result;
        }

        async public Task UpdateTodo(TodoDto todo)
        {
            var daoObj = await _dataProvider.Select<Todo>().FirstOrDefaultAsync(x => x.Id == todo.Id);
            if (daoObj != null)
            {
                _mapper.Map<TodoDto, Todo>(todo, daoObj);
                await _dataProvider.Update<Todo>(daoObj);
                await _dataProvider.SaveAsync();
            }
        }
    }
}