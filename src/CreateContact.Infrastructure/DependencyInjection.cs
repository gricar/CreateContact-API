using CreateContact.Application.Persistence;
using CreateContact.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreateContact.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigurePersistenceServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("Database")));

            services.AddScoped<IContactRepository, ContactRepository>();

            return services;
        }
    }
}
