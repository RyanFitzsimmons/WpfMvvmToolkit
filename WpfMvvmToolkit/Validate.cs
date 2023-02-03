using System.Collections.Generic;

namespace WpfMvvmToolkit
{
    public static class Validate
    {
        public static IEnumerable<string> String(string? value, bool canBeEmpty = true, int maxLength = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                yield return "Cannot be empty";
            }

            if (maxLength > 0 && value?.Length > maxLength)
            {
                yield return $"Cannot be longer than {maxLength}";
            }
        }

        public static IEnumerable<string> Decimal(decimal value, decimal min = decimal.MinValue, decimal max = decimal.MaxValue)
        {
            if (value < min)
            {
                yield return $"Cannot be less than {min}";
            }

            if (value > max)
            {
                yield return $"Cannot be greater than {max}";
            }
        }
    }
}
