using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utility.Tools
{
    public static class RandomGenerator
    {
        private static Random _random = new Random();

        public static int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        // Generates a random string with a given size.    
        public static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        public static string RandomPassword()
        {
            var passwordBuilder = new StringBuilder();

            passwordBuilder.Append(RandomString(4, true));

            passwordBuilder.Append(RandomNumber(1000, 9999));

            passwordBuilder.Append(RandomString(2));
            return passwordBuilder.ToString();
        }

        public static long GetUniqueInt(int size)
        {
            byte[] value = Guid.NewGuid().ToByteArray();
            long x = Math.Abs(BitConverter.ToInt64(value, 0));
            return long.Parse(x.ToString().Substring(0, size));
        }
    }
}
