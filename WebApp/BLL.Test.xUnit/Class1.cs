using DAO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BLL.Test.xUnit
{
    public class EntityTest
    {

        readonly ServiceProvider _provider;
        IGetTodoService GetTodoService => _provider?.GetService<IGetTodoService>();

        public EntityTest()
        {
            var myConfiguration = new Dictionary<string, string>{
    {"ConnectionStrings:DefaultConnection", "Host = localhost; Port = 5432; Database = TodoDb; Username = postgres; Password = changeme; "} };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();            
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddSingleton<IConfiguration>(configuration)
                .AddDao(configuration)
                .AddBllClasses();

            _provider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        async public void TodoListTest()
        {
            var srv = GetTodoService; 
            var todos = await srv.ListAsync(null);
            Assert.NotEmpty(todos);
        }

        [Fact]
        async public void CommentListTest()
        {
            var srv = GetTodoService; 
            var comments = await srv.Async(1);
            Assert.NotEmpty(comments);
            Assert.Equal(2, comments.Count);
        }

        [Fact]
        async public void GetTodoByIdTest()
        { 
            var srv = GetTodoService; 
            var todo = await srv.GetAsync(1);
            Assert.NotNull(todo);
            todo = await srv.GetAsync(-1);
            Assert.Null(todo);
        }

        [Fact]
        async public void InsertTodoTest()
        {
            var srv = GetTodoService;
            var title = Guid.NewGuid().ToString();

            var currentTodoList = await srv.ListAsync(null);
            Assert.NotEmpty(currentTodoList);
            await srv.AddTodoAsync(new TodoDto
            {
                Title = title.ToString(),
                Category = "dkfd",
                Color = "red",
                Created = DateTime.Now,
            });
            var newTodoList = await srv.ListAsync(null);
            Assert.True((newTodoList.Count - currentTodoList.Count) == 1);
            var addedObj = newTodoList.FirstOrDefault(x => x.Title == title);
            Assert.NotNull(addedObj);
            Assert.True(addedObj.Id > 0);
        }

        [Fact]
        async public void UpdateTodoTest()
        {
            var srv = GetTodoService;
            var currentTodoList = await srv.ListAsync(null);
            var obj = currentTodoList.OrderBy(x => x.Id).Last();
            var title = Guid.NewGuid().ToString();

            obj.Title = title;
            await srv.UpdateTodo(obj);

            var updatedObj = await srv.GetAsync(obj.Id);

            Assert.NotNull(updatedObj);
            Assert.Equal(title, obj.Title);
            Assert.NotEqual(obj, updatedObj);
        }
        
        [Fact]
        async public void AddCommentTest()
        {
            var srv = GetTodoService;
            var currentTodoList = await srv.ListAsync(null);
            var obj = currentTodoList.OrderBy(x => x.Id).Last();
            var commentText = Guid.NewGuid().ToString();
            var comment = new CommentDto
            {
                Text = commentText,
            };

            
            await srv.AddCommentAsync(obj.Id, comment);

            var updatedObj = await srv.GetAsync(obj.Id);

            Assert.NotNull(updatedObj);
            Assert.NotEmpty(updatedObj.Comments);
            Assert.Contains(updatedObj.Comments,x=>x.Text == commentText);
        }

        [Fact]
        async public void DeleteTodoTest()
        {
            var srv = GetTodoService;
            var currentTodoList = await srv.ListAsync(null);
            var obj = currentTodoList.OrderBy(x => x.Id).Last();

            await srv.DeleteTodoAsync(obj.Id);

            var deletedObj = await srv.GetAsync(obj.Id);
            Assert.Null(deletedObj);
        }
    }
}