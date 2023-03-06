using DAO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp
{

    public sealed class BDLogger : ILogger
    {
        readonly IDataProvider _dataprovider;
        public BDLogger(IDataProvider dataprovider)
        {
            _dataprovider = dataprovider;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var dao = _dataprovider.Select<Todo>().First();
        }
    }


    public sealed class DBLoggerProvider : ILoggerProvider
    {
        readonly IDataProvider _dataprovider;
        public DBLoggerProvider(IDataProvider dataprovider)
        {
            _dataprovider = dataprovider;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new BDLogger(_dataprovider);
        }

        public void Dispose()
        {
        }
    }

    public static class DBLoggerExtensions
    {
        public static ILoggerFactory AddDBLogger(this ILoggerFactory factory, IDataProvider dataProvider)
        {
            factory.AddProvider(new DBLoggerProvider(dataProvider));
            return factory;
        }
    }

}
