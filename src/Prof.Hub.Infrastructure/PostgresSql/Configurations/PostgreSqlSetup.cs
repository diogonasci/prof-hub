using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
public class PostgreSqlSetup : IConfigureOptions<PostgreSqlSettings>
{
    private const string ConfigurationSectionName = "PostgreSqlSettings";
    private readonly IConfiguration _configuration;

    public PostgreSqlSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(PostgreSqlSettings settings)
    {
        var connectionString = _configuration.GetConnectionString(ConfigurationSectionName);

        settings.ConnectionString = connectionString;

        _configuration.GetSection(ConfigurationSectionName).Bind(settings);
    }
}
