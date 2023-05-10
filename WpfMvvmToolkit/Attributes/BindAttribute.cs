using System;

namespace WpfMvvmToolkit.Attributes
{
    public class BindAttribute : Attribute
    {
        public BindAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }

        public Type InterfaceType { get; }
    }
}
