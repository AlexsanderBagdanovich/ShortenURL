using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShortenURL.Application;

namespace ShortenURL.Infrastructure;
// Настройка инфраструктурных зависимостей. Сюда вынесено подключение к БД и регистрация сервисов репозитория.
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        var connection = config.GetConnectionString("Default");

        services.AddDbContextPool<AppDbContext>(options =>
            options.UseMySql(connection,
                ServerVersion.AutoDetect(connection)));

        services.AddScoped<IShortLinkRepository, ShortLinkRepository>();
        services.AddScoped<ShortLinkService>();
        services.AddSingleton<ShortCodeGenerator>();

        return services;
    }
}
