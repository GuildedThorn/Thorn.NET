using ThornData.Models.Bedrock;
using ThornData.Services.Bedrock;

var builder = WebApplication.CreateBuilder(args); {
    
    // add services to the process
    var services = builder.Services;

    // add cross-origin resource sharing services
    services.AddCors();
    
    // add Factions Database Settings to the container
    services.Configure<FactionsDatabaseSettings>(
        builder.Configuration.GetSection("Bedrock:FactionsDatabase"));

    services.Configure<UsersDatabaseSettings>(
        builder.Configuration.GetSection("Bedrock:UsersDatabase"));
    
    // Add the Data Services into dependency injection
    services.AddSingleton<FactionsService>();
    services.AddSingleton<UserService>();

    // enable the use of controllers for the app
    services.AddControllers();

    // enable the use of versioning for the app
    // services.AddApiVersioning(setup => {
    //     setup.DefaultApiVersion = new ApiVersion(1, 0);
    //     setup.AssumeDefaultVersionWhenUnspecified = true;
    //     setup.ReportApiVersions = true;
    //     setup.ApiVersionReader = new HeaderApiVersionReader("version");
    // });

    services.AddAutoMapper(typeof(Program));

    // configure api explorer
    services.AddEndpointsApiExplorer();
    
    // introduce swagger api to the app
    services.AddSwaggerGen();
}

// build the api application
var app = builder.Build();
using (var scope = app.Services.CreateScope()) {

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