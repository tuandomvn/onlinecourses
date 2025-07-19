namespace Acme.OnlineCourses.Helpers
{
    public static class PasswordGenerator
    {
        public static string GenerateSecurePassword(int length = 12)
        {
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercase = "abcdefghijklmnopqsrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()-_=+<>?";

            var random = new Random();

            var password = new List<char>
            {
              uppercase[random.Next(uppercase.Length)],
              lowercase[random.Next(lowercase.Length)],
              digits[random.Next(digits.Length)],
              specialChars[random.Next(specialChars.Length)]
            };

            string allChars = uppercase + lowercase + digits + specialChars;
            for (int i = password.Count; i < length; i++)
            {
                password.Add(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle to prevent predictable placement
            return new string(password.OrderBy(_ => random.Next()).ToArray());
        }
    }
}
