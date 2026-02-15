namespace ShortenURL.Application;

public static class UrlValidator
{
    public static void Validate(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be empty");                 // Пустые строки не имеют смысла для коротких ссылок и могут вызвать ошибки при сохранении.

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            throw new ArgumentException("Invalid URL format");                  // Нужно убедиться, что переданный URL корректный и полон (схема + хост), иначе редирект не будет работать.

        if (uri.Scheme != Uri.UriSchemeHttp &&
            uri.Scheme != Uri.UriSchemeHttps)
            throw new ArgumentException("Only HTTP/HTTPS allowed");             // Поддерживаем только HTTP и HTTPS, чтобы избежать небезопасных или неработающих схем (например, ftp://).
    }
}
