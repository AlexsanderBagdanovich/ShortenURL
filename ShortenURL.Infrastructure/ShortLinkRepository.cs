using Microsoft.EntityFrameworkCore;
using ShortenURL.Application;
using ShortenURL.Domain;

namespace ShortenURL.Infrastructure;

public class ShortLinkRepository : IShortLinkRepository
{
    private readonly AppDbContext _context;

    public ShortLinkRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(ShortLink entity)
        => _context.ShortLinks.AddAsync(entity).AsTask();

    public Task<ShortLink?> GetByCodeAsync(string code)
        => _context.ShortLinks
            .AsNoTracking()                             // Для обычного чтения нет смысла отслеживать объект EF, это снижает нагрузку и ускоряет выборку
            .FirstOrDefaultAsync(x => x.Code == code);

    public Task<ShortLink?> GetByIdAsync(Guid id)
        => _context.ShortLinks.FindAsync(id).AsTask();  // FindAsync лучше для поиска по первичному ключу, т.к. сначала проверяет кэш контекста

    public Task<List<ShortLink>> GetAllAsync()
        => _context.ShortLinks
            .AsNoTracking()                             // Чтобы уменьшить накладные расходы при рендере таблицы
            .OrderByDescending(x => x.CreatedAt)        // Сортировка по дате нужна для UX: новые ссылки сверху
            .ToListAsync();

    public Task<bool> CodeExistsAsync(string code)
        => _context.ShortLinks.AnyAsync(x => x.Code == code);  // Используем AnyAsync, чтобы проверить уникальность кода до создания новой ссылки

    public Task DeleteAsync(ShortLink entity)
    {
        _context.ShortLinks.Remove(entity);             // Без SaveChanges потому что откладываем сохранение, чтобы можно было объединять несколько операций в транзакцию
        return Task.CompletedTask;
    }

    public Task IncrementClicksAsync(string code)

        // Чтобы не подгружать сущность и не трогать контекст при каждом клике
        => _context.Database.ExecuteSqlRawAsync(
            "UPDATE ShortLinks SET ClickCount = ClickCount + 1 WHERE Code = {0}",
            code);

    public Task SaveChangesAsync()
        => _context.SaveChangesAsync();
}
