using System;
using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.Utils
{
    public static class ExtensionMethods
    {
        public static bool IsNotNullOrEmpty(this string inputtedString)
        {
            bool hasValue = false;
            if (!string.IsNullOrEmpty(inputtedString) && !string.IsNullOrWhiteSpace(inputtedString))
                hasValue = true;
            return hasValue;
        }

        public static bool IsNullOrEmpty(this string inputtedString)
        {
            bool hasValue = false;
            if (string.IsNullOrEmpty(inputtedString) || string.IsNullOrWhiteSpace(inputtedString))
                hasValue = true;
            return hasValue;
        }

        public static string StrToUpper(this string inputtedString)
        {
            if (!string.IsNullOrEmpty(inputtedString) && !string.IsNullOrWhiteSpace(inputtedString))
                inputtedString = inputtedString.Trim().ToUpper();
            return inputtedString;
        }

        public static string StrToLower(this string inputtedString)
        {
            if (!string.IsNullOrEmpty(inputtedString) && !string.IsNullOrWhiteSpace(inputtedString))
                inputtedString = inputtedString.Trim().ToLower();
            return inputtedString;
        }

        public static string ToEnumString(this Enum enumValue)
        {
            string value = string.Empty;
            if (enumValue != null && enumValue is Enum)
            {
                value = Convert.ToString(enumValue).Replace("_", " ");
            }
            return value;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
