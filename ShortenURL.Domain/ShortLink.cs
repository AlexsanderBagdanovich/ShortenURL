namespace ShortenURL.Domain;

public class ShortLink
{
    public Guid Id { get; private set; }
    public string OriginalUrl { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public int ClickCount { get; private set; }

    private ShortLink() { } // EF требует приватный конструктор

    public ShortLink(string originalUrl, string code)
    {
        Id = Guid.NewGuid();
        OriginalUrl = originalUrl;
        Code = code;
        CreatedAt = DateTime.UtcNow;
        ClickCount = 0;
    }

    public void Update(string newUrl)
    {
        // Логику обновления держим внутри сущности, чтобы инкапсулировать изменения состояния.
        OriginalUrl = newUrl;
    }
}
