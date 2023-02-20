using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Exceptions
{
    public class IncompatibleTypeRegistrationException : Exception
    {
        public IncompatibleTypeRegistrationException(Type type1, Type type2) 
            : base($"{type2.Name} is not of type {type1.Name}")
        {
        }
    }
}
