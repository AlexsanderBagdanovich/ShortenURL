using System.Security.Cryptography;

namespace ShortenURL.Application;

public class ShortCodeGenerator
{
    private const string Alphabet =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public string Generate(int length = 8)
    {
        // Используем криптографический RNG, чтобы код невозможно было предугадать
 
        Span<byte> buffer = stackalloc byte[length];
        RandomNumberGenerator.Fill(buffer);

        var chars = new char[length];
        for (int i = 0; i < length; i++)
            chars[i] = Alphabet[buffer[i] % Alphabet.Length];

        return new string(chars);
    }
}
