using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public static class DiContainerConfig
    {
        public static IServiceCollection AddDao(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TodoDbContext>(o => o.UseNpgsql(connectionString));
            //services.BuildServiceProvider().GetService<TodoDbContext>().Database.Migrate(); // DB automigration on start enable
            services.AddScoped<IDataProvider, DataProvider>();            
            return services;
        }        
    }    
}
