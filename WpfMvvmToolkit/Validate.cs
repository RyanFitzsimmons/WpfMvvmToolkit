using System.Collections.Generic;
using System.IO;

namespace WpfMvvmToolkit
{
    public static class Validate
    {
        public static IEnumerable<string> String(string? value, bool canBeEmpty = true, int minLength = 0, int maxLength = 0)
        {
            if (!canBeEmpty && string.IsNullOrWhiteSpace(value))
            {
                yield return "Cannot be empty";
            }

            if (minLength > 0 && value?.Length < minLength)
            {
                yield return $"Cannot be shorter than {minLength}";
            }

            if (maxLength > 0 && value?.Length > maxLength)
            {
                yield return $"Cannot be longer than {maxLength}";
            }
        }

        public static IEnumerable<string> Integer(int value, int min = int.MinValue, int max = int.MaxValue)
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

        public static IEnumerable<string> Directory(string? directoryPath, bool canBeEmpty = false, bool shouldExist = false)
        {
            if ((shouldExist || !canBeEmpty) && string.IsNullOrWhiteSpace(directoryPath))
            {
                yield return "The path cannot be empty";
                yield break;
            }

            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                yield break;
            }

            DirectoryInfo directory = new(directoryPath);

            if (shouldExist && !directory.Exists)
            {
                yield return $"The directory does not exist {directory.FullName}";
            }

            if (!shouldExist && directory.Exists)
            {
                yield return $"The directory already exists {directory.FullName}";
            }
        }

        public static IEnumerable<string> File(string? filePath, bool canBeEmpty = false, bool shouldExist = false)
        {
            if ((shouldExist || !canBeEmpty) && string.IsNullOrWhiteSpace(filePath))
            {
                yield return "The path cannot be empty";
                yield break;
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                yield break;
            }

            FileInfo file = new(filePath);

            if (shouldExist && !file.Exists)
            {
                yield return $"The file does not exist {file.FullName}";
            }

            if (!shouldExist && file.Exists)
            {
                yield return $"The file already exists {file.FullName}";
            }
        }

        public static IEnumerable<string> Object(object? obj, bool canBeNull = false)
        {
            if (obj == null && !canBeNull)
            {
                yield return "The value cannot be null";
            }
        }
    }
}
