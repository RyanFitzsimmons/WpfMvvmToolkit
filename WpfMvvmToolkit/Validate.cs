using System.Collections.Generic;
using System.IO;

namespace WpfMvvmToolkit
{
    public static class Validate
    {
        public static IEnumerable<string> String(string? value, bool canBeEmpty = true, int maxLength = 0)
        {
            if (!canBeEmpty && string.IsNullOrWhiteSpace(value))
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

        public static IEnumerable<string> Directory(DirectoryInfo directory, bool canBeEmpty = false, bool shouldExist = false)
        {
            if (!canBeEmpty && string.IsNullOrWhiteSpace(directory.FullName))
            {
                yield return "The path cannot be empty";
            }

            if (shouldExist && !directory.Exists)
            {
                yield return $"The directory does not exist {directory.FullName}";
            }

            if (!shouldExist && directory.Exists)
            {
                yield return $"The directory already exists {directory.FullName}";
            }
        }

        public static IEnumerable<string> File(FileInfo file, bool canBeEmpty = false, bool shouldExist = false)
        {
            if (!canBeEmpty && string.IsNullOrWhiteSpace(file.FullName))
            {
                yield return "The path cannot be empty";
            }

            if (shouldExist && !file.Exists)
            {
                yield return $"The file does not exist {file.FullName}";
            }

            if (!shouldExist && file.Exists)
            {
                yield return $"The file already exists {file.FullName}";
            }
        }
    }
}
