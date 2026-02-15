using ShortenURL.Domain;

namespace ShortenURL.Application;

public class ShortLinkService
{
    private readonly IShortLinkRepository _repository;
    private readonly ShortCodeGenerator _generator;

    public ShortLinkService(
        IShortLinkRepository repository,
        ShortCodeGenerator generator)
    {
        _repository = repository;
        _generator = generator;
    }

    public async Task<ShortLink> CreateAsync(string url)
    {
        UrlValidator.Validate(url);

        string code;

        
        do
        {
            code = _generator.Generate();                        // Retry нужен, потому что генерация случайная.Коллизии крайне редки, но мы обязаны их обработать 
        }
        while (await _repository.CodeExistsAsync(code));

        var entity = new ShortLink(url, code);

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return entity;
    }
}
