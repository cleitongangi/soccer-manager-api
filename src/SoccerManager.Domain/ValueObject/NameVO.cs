using SoccerManager.Domain.Interfaces.Services;

namespace SoccerManager.Domain.ValueObject
{
    public class NameVO
    {
        public static string GenerateName(int? maxLen = null)
        {
            Random r = new Random();
            if (!maxLen.HasValue)
            {
                maxLen = r.Next(4, 10);
            }

            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "y" };
            string Name = string.Empty;
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; // b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < maxLen)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
        }
    }
}
