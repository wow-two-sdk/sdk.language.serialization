using WoW2.Sdk.Language.Serialization.Json.Abstractions.Brokers;
using WoW2.Sdk.Language.Serialization.Json.Newtonsoft.Brokers;
using Microsoft.Extensions.DependencyInjection;

namespace WoW2.Sdk.Language.Serialization.Json.Newtonsoft.DependencyInjection.Configurations;

/// <summary>
/// Provides extension methods to configure the serialization provider.
/// </summary>
public static class InfraConfigurations
{
    /// <summary>
    /// Configures the serialization provider to use Newtonsoft JSON serialization.
    /// </summary>
    /// <param name="services"></param>
    public static void AddNewtonsoftJsonSerializer(this IServiceCollection services)
    {
        services
            .AddSingleton<INewtonsoftJsonSerializationSettingsProvider, NewtonsoftJsonSerializationSettingsProvider>()
            .AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
    }
}