using Microsoft.EntityFrameworkCore;
using ThornData.Contexts;
using ThornData.Models;

var builder = WebApplication.CreateBuilder(args);
{
    // add services to the process
    var services = builder.Services;
    var env = builder.Environment;
    
    // bind settings section to APISettings class
    services.Configure<APISettings>(builder.Configuration.GetSection("Settings"));

    // add database context based on Config Decision
    switch (builder.Configuration.GetSection("Settings:Database").Value) { 
        case "MariaDB":
            services.AddDbContext<DataContext>();
            break;
        case "SQLite":
            services.AddDbContext<SqLiteDataContext>();
            break;
    }

    // add cross-origin resource sharing services
    services.AddCors();
    
    // enable the use of controllers for the app
    services.AddControllers();

    services.AddAutoMapper(typeof(Program));

    // configure api explorer
    services.AddEndpointsApiExplorer();
    
    // introduce swagger api to the app
    services.AddSwaggerGen();
}

// build the api application
var app = builder.Build();
using (var scope = app.Services.CreateScope()) {
    
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    // automatically migrate database to the new proccess
    dataContext.Database.Migrate();
    
    var env = builder.Environment;
    
    // allow connections from any origin using any method or header
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    switch (env.EnvironmentName) {
        case "Development":
            // enable the swagger UI
            app.UseSwagger();
            app.UseSwaggerUI();
            break;
        case "Production":
            break;
    }
}

// automatically redirect http connections to ssl
app.UseHttpsRedirection();

// allows authentication methods in the API
app.UseAuthorization();

// automatically map API Controllers
app.MapControllers();

// run the application
app.Run();