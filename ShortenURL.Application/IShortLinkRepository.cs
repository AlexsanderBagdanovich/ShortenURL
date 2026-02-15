using ShortenURL.Domain;

namespace ShortenURL.Application;

public interface IShortLinkRepository
{
    Task AddAsync(ShortLink entity);
    Task<ShortLink?> GetByCodeAsync(string code);
    Task<ShortLink?> GetByIdAsync(Guid id);
    Task<List<ShortLink>> GetAllAsync();
    Task<bool> CodeExistsAsync(string code);
    Task DeleteAsync(ShortLink entity);
    Task IncrementClicksAsync(string code);
    Task SaveChangesAsync();
}
