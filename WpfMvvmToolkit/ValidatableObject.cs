using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using WpfMvvmToolkit.Attributes;

namespace WpfMvvmToolkit
{
    public class ValidatableObject : ObservableObject, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propertyErrors = new();
        public bool HasErrors => _propertyErrors.Any(x => x.Value.Count > 0);

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            ArgumentNullException.ThrowIfNull(propertyName);

            if (!_propertyErrors.ContainsKey(propertyName))
            {
                yield break;
            }

            foreach (var error in _propertyErrors[propertyName])
            {
                yield return error;
            }
        }

        protected void OnPropertyErrorsChanged([CallerMemberName] string? propertyName = null)
        {
            ArgumentNullException.ThrowIfNull(propertyName);

            ErrorsChanged?.Invoke(this, new(propertyName));
        }

        /// <summary>
        /// Clears any errors then adds the new error messages and calls <see cref="OnPropertyErrorsChanged(string?)"/>
        /// </summary>
        /// <param name="messages">The error messages</param>
        /// <param name="propertyName">The property name</param>
        protected void SetPropertyErrors(IEnumerable<string> messages, [CallerMemberName] string? propertyName = null)
        {
            ArgumentNullException.ThrowIfNull(propertyName);

            if (!_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Add(propertyName, new());
            }

            _propertyErrors[propertyName].Clear();
            _propertyErrors[propertyName].AddRange(messages);
            OnPropertyErrorsChanged(propertyName);
        }

        /// <summary>
        /// Forces validation on all properties
        /// </summary>
        public void ValidateAllProperties()
        {
            foreach (var property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty))
            {
                var ignore = property.GetCustomAttributes(true).SingleOrDefault(x => x.GetType() == typeof(ValidationIgnoreAttribute));
                if (ignore != null)
                {
                    continue;
                }

                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(this));
                }
            }
        }
    }
}
