using System;
using System.IO;

namespace WpfMvvmToolkit.Exceptions
{
    public class NotLoadedException : Exception
    {
        public NotLoadedException(string className)
            : base($"The {className} is not loaded.")
        {

        }

        public static void ThrowIfNot(bool isLoaded, [System.Runtime.CompilerServices.CallerFilePath] string className = "")
        {
            if (!isLoaded)
            {
                Throw(Path.GetFileNameWithoutExtension(className));
            }
        }

        private static void Throw(string className)
        {
            throw new NotLoadedException(className);
        }
    }
}
