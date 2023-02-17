using System;
using System.IO;

namespace WpfMvvmToolkit.Exceptions
{
    public class AlreadyLoadedException : Exception
    {
        public AlreadyLoadedException(string className)
            : base($"The {className} is already loaded.")
        {

        }

        public static void ThrowIf(bool isLoaded, [System.Runtime.CompilerServices.CallerFilePath] string className = "")
        {
            if (isLoaded)
            {
                Throw(Path.GetFileNameWithoutExtension(className));
            }
        }

        private static void Throw(string className)
        {
            throw new AlreadyLoadedException(className);
        }
    }
}
