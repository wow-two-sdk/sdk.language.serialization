using WoW2.Sdk.Language.Serialization.Json.Abstractions.Brokers;
using WoW2.Sdk.Language.Serialization.Json.System.Brokers;
using Microsoft.Extensions.DependencyInjection;

namespace WoW2.Sdk.Language.Serialization.Json.System.DependencyInjection.Configurations;

/// <summary>
/// Provides extension methods to configure the serialization provider.
/// </summary>
public static class InfraConfigurations
{
    /// <summary>
    /// Configures the serialization provider to use System.Text.Json serialization.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    public static void AddSystemTextJsonSerializer(this IServiceCollection services)
    {
        services
            .AddSingleton<ISystemJsonSerializationOptionsProvider, SystemJsonSerializationOptionsProvider>()
            .AddSingleton<IJsonSerializer, SystemJsonSerializer>();
    }
}