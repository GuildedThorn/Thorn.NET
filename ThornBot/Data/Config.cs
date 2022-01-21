using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Yaml;

namespace ThornBot.Data; 

public class Config {

    public static IConfiguration _config;

    public static void InitConfig() {
        // Build a yaml config to hold important info
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory, "Resources"))
            .AddYamlFile("config.yaml", true);
        _config = builder.Build();
    }

}
