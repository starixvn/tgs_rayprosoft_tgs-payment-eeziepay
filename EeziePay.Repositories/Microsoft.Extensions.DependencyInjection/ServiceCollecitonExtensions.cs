using System;
using Microsoft.EntityFrameworkCore;
using Tgs.Payment.EeziePay;
using Tgs.Payment.EeziePay.Repositories;
using Tgs.Payment.EeziePay.Repositories.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollecitonExtensions
    {
        public static IServiceCollection AddEeziePayRepositories(
            this IServiceCollection services,
            Action<IServiceProvider, DbContextOptionsBuilder> configure,
            int poolSize = 128)
        {
            return services
            .AddDbContext<TgsContext>(configure)
            .AddTransient<ITgsRepository, TgsRepository>();
        }
    }
}