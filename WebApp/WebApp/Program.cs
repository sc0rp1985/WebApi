using BLL;
using DAO;
using Mappers;
using WebApp;
using WebApp.CustomMiddlewares;
using WebApp.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<LoggingFilter>();
    options.Filters.Add<ExceptionLoggingFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDao(builder.Configuration);
builder.Services.AddBllClasses();
builder.Services.AddMapper();


var app = builder.Build();



//app.UseHttpLogging();
app.UseExceptionHandlerMiddleware();

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

app.UseCustomMiddleware();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
