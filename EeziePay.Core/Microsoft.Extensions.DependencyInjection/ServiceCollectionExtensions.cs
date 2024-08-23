using System;
using System.Reflection;
using Tgs.Payment.EeziePay;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEeziePay(
       this IServiceCollection services,
       Action<EeziePayOptions> configureOptions,
       Action<IHttpClientBuilder> ConfigureAiClient)
        {
            ConfigureAiClient?.Invoke(services.AddHttpClient<EeziePayClient>());

            return services
                .AddOptions<EeziePayOptions>()
                .Configure(configureOptions)
                .Services
                .RegisterAllServiceUtilityTypes(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient)
                .AddTransient<EeziePayHasher>()
                .AddSingleton<EeziePaySerializer>();
        }
    }
}