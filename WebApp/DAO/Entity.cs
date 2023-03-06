using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace DAO
{
    public class Todo
    {        
        public int Id { get; set; }
        public string Title { get; set; } 
        public DateTime Created { get; set; }

        public string Category { get; set; }
        public string Color { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }

    public class Comment 
    {        
        public int Id { get; set; }
        public string Text { get; set; }
        public int TodoId { get; set; }
        public Todo Todo { get; set; }
    }

    public class Log 
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

    /*
     
     CREATE TABLE logs
( 
    Id serial primary key,
    Application character varying(100) NULL,
    Logged text,
    Level character varying(100) NULL,
    Message character varying(8000) NULL,
    Logger character varying(8000) NULL, 
    Callsite character varying(8000) NULL, 
    Exception character varying(8000) NULL
)
     */



    public class TodoDbContext : DbContext
    { 
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Log> Logs { get; set; }

        public TodoDbContext()
        {
            Database.EnsureCreated();
        }

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            UpdateStructure(builder);            
            SeedData(builder);            
            base.OnModelCreating(builder);
        }

        private void UpdateStructure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
                .HasMany(i => i.Comments)
                .WithOne(g => g.Todo);

            modelBuilder.Entity<Todo>()
                .HasIndex(uk => new { uk.Title, uk.Category }).IsUnique();
        }

        private void SeedData(ModelBuilder builder) 
        {
            var todos = new List<Todo> {
                new()
                {
                    Id = 1,
                    Title = "Create a ticket",
                    Category = "analytics",
                    Color = "red",                    
                    // Created = DateTime.SpecifyKind(DateTime.Now,DateTimeKind.Utc),
                    Created = DateTime.Now,
                },
                new()
                {
                    Id = 2,
                    Title = "Request information",
                    Category = "bookkeeping",
                    Color = "green",
                    //Created = DateTime.SpecifyKind(DateTime.Now,DateTimeKind.Utc),
                    Created = DateTime.Now,
                }
            };

            var comments = new List<Comment> {

                        new()
                        {
                            Id = 1,
                            Text = "Test1",
                            TodoId = 1,
                        },
                        new()
                        {
                            Id = 2,
                            Text = "Test2",
                            TodoId = 1
                        },
            };

            builder.Entity<Todo>().HasData(todos.ToArray());
            builder.Entity<Comment>().HasData(comments.ToArray());
        }
    }

}