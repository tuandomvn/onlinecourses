using System.Security.Cryptography;

namespace Acme.OnlineCourses.Helpers;

public static class PasswordGenerator
{
    public static string GenerateSecurePassword(int length = 12)
    {
        if (length < 8)
            throw new ArgumentException("Password length must be at least 4 characters to include all character types.", nameof(length));

        const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowercase = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string specialChars = "!@#$%^&*()-_=+<>?";

        using var rng = RandomNumberGenerator.Create();

        var password = new List<char>
        {
            GetRandomChar(uppercase, rng),
            GetRandomChar(lowercase, rng),
            GetRandomChar(digits, rng),
            GetRandomChar(specialChars, rng)
        };

        string allChars = uppercase + lowercase + digits + specialChars;
        for (int i = password.Count; i < length; i++)
        {
            password.Add(GetRandomChar(allChars, rng));
        }

        // Shuffle to prevent predictable placement
        return new string(ShuffleArray(password.ToArray(), rng));
    }

    private static char GetRandomChar(string chars, RandomNumberGenerator rng)
    {
        var randomBytes = new byte[4];
        rng.GetBytes(randomBytes);
        var randomValue = BitConverter.ToUInt32(randomBytes, 0);
        return chars[(int)(randomValue % (uint)chars.Length)];
    }

    private static T[] ShuffleArray<T>(T[] array, RandomNumberGenerator rng)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            var randomBytes = new byte[4];
            rng.GetBytes(randomBytes);
            var randomValue = BitConverter.ToUInt32(randomBytes, 0);
            int j = (int)(randomValue % (uint)(i + 1));
            
            (array[i], array[j]) = (array[j], array[i]);
        }
        return array;
    }
}
