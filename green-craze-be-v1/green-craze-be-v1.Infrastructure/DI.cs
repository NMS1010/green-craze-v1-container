using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Domain.Entities;
using green_craze_be_v1.Infrastructure.Data.Context;
using green_craze_be_v1.Infrastructure.Repositories;
using green_craze_be_v1.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace green_craze_be_v1.Infrastructure
{
    public static class DI
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddRepositories();
            services.AddDbContextSetup(configuration);
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddScoped<IStatisticRepository, StatisticRepository>();
        }

        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<AppDBContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return webApp;
        }

        public static void AddDbContextSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(options =>
                options.UseMySQL(configuration.GetConnectionString("AppDBContext")));
            services.AddIdentity<AppUser, AppRole>(opts =>
            {
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequiredLength = 1;
                opts.Password.RequireDigit = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IUploadService, UploadService>()
                .AddScoped<IMailService, MailService>();
        }
    }
}