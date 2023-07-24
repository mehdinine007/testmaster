using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Esale.Core.Utility.Tools
{
    public static class AppTools
    {
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString().ToUpper();
        }

        public static string PreapareStringForFullTextSearch(string text, bool isExactWord)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }

            //if (text.StartsWith("\"") && text.EndsWith("\""))
            if (isExactWord)
                return "\"" + text.Replace("\"", " ") + "\"";
            else
                text = text.Replace("\"", " ");

            Regex regex = new Regex(@"[ ]{2,}", RegexOptions.None);

            text = regex.Replace(text, @" ");
            text = text.Replace("‌", " ");
            text = text.Trim();

            string[] words = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //words = Tools.RemovePersianStoplist(words);

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = "\"" + words[i] + "*\"";
            }

            text = string.Join(" ", words);

            if (!text.Contains(" "))
                return text;
            else
                return text.Replace(" ", " near ");
        }

    }
}
