using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GMEngine.String.Extension
{
    public static class GMStringExtension
    {
        /// <summary>
        /// Convert Camel Case string to Title Case
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertToTitleCase(this string input)
        {
            // Use a regular expression to split the input string into words
            string[] words = Regex.Split(input, @"(?<!^)(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])");

            // Capitalize the first letter of each word and join them with spaces
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i].ToLower());
            }

            string formattedString = string.Join(" ", words);

            return formattedString;
        }

    }

}

