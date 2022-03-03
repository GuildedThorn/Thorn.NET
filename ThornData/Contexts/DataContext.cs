using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace ThornData.Contexts; 

public class DataContext : DbContext {

    protected readonly IConfiguration Configuration;
    
    public DataContext(IConfiguration configuration) {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        options.UseMySql(Configuration.GetConnectionString("MariaDB"),
            ServerVersion.AutoDetect(Configuration.GetConnectionString("MariaDB")));
        options.LogTo(Console.WriteLine, LogLevel.Information);
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }

}