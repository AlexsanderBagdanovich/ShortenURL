using Microsoft.EntityFrameworkCore;
using ShortenURL.Domain;

namespace ShortenURL.Infrastructure;
//Контекст базы данных для приложения. Инкапсулирует работу с таблицами и схемой БД через EF Core

public class AppDbContext : DbContext
{
    public DbSet<ShortLink> ShortLinks => Set<ShortLink>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ShortLink>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Code) 
                .HasMaxLength(8)            // Длина ограничена 8 символами, чтобы контролировать размер короткой ссылки.
                .IsRequired();              // IsRequired нужен, чтобы EF не разрешал пустые коды.

            
            e.HasIndex(x => x.Code)         // Для быстрого поиска по короткой ссылке и предотвращения коллизий
                .IsUnique();
        });
    }
}
