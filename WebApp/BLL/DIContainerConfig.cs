using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class DIContainerConfig
    {
        public static IServiceCollection AddBllClasses(this IServiceCollection services)
        {                     
            services.AddScoped<IGetTodoService, GetTodoService>();
            services.AddScoped<ILogService, LogService>();
            return services;
        }
    }
}
