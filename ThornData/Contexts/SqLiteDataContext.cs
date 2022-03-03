using Microsoft.Extensions.Configuration;

namespace ThornData.Contexts; 

using Microsoft.EntityFrameworkCore;

public class SqLiteDataContext : DataContext {

    public SqLiteDataContext(IConfiguration configuration) : base(configuration) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        options.UseSqlite("Thorn.db");
    }
}