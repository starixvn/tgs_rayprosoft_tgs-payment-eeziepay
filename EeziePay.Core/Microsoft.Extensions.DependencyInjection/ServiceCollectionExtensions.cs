using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tgs.Payment.EeziePay;
using Tgs.Payment.EeziePay.MessageQueue;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEeziePay(this IServiceCollection services, Action<EeziePayOptions, IServiceProvider> configure)
            => services
                .AddOptions<EeziePayOptions>()
                .Configure(configure)
                .Services
                .AddHttpClient<EeziePayClient>()
                .ConfigureHttpClient((sp, httpClient) =>
                {
                    var options = sp.GetRequiredService<IOptions<EeziePayOptions>>().Value;

                    httpClient.BaseAddress = options.PaymentApiUrl;
                })
                .Services
                .RegisterAllServiceUtilityTypes(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient)
                .AddTransient<EeziePayHasher>()
                .AddSingleton<EeziePaySerializer>()
                .AddNatsMessageQueue(config =>
                {
                    config
                        .ConfigQueueOptions((options, sp) =>
                        {
                            var applePayOptions = sp.GetRequiredService<IOptions<EeziePayOptions>>().Value;

                            options.Url = applePayOptions.NatsUrl;
                        })
                        .AddProcessor<DepositProcessor>("tgs.payment.eeziepay.deposit", "tgs.payment.eeziepay.deposit")
                        .AddProcessor<WithdrawProcessor>("tgs.payment.eeziepay.withdraw", "tgs.payment.eeziepay.withdraw");
                })
                .AddPaymentClient();
    }
}