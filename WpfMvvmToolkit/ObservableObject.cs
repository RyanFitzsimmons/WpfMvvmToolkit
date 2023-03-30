using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WpfMvvmToolkit
{
    public class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private readonly Dictionary<string, List<Action>> _propertyChangingDelegates = new();
        private readonly Dictionary<string, List<Action>> _propertyChangedDelegates = new();
        private readonly List<string> _ignoredPropertyNames = new();
        private bool _hasChanged;

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsPropertyChangeMonitoringEnabled { get; set; } = true;
        public virtual bool HasChanged
        {
            get => _hasChanged;
            set => SetProperty(ref _hasChanged, value);
        }

        protected virtual bool SetProperty<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);
            InvokePropertyChangingDelegates(propertyName);
            field = newValue;
            OnPropertyChanged(propertyName);
            InvokePropertyChangedDelegates(propertyName);
            return true;
        }

        private void InvokePropertyChangingDelegates(string? propertyName)
        {
            if (propertyName == null)
            {
                return;
            }

            if (!_propertyChangingDelegates.ContainsKey(propertyName))
            {
                return;
            }

            foreach (var propertyChangingDelegate in _propertyChangingDelegates[propertyName])
            {
                propertyChangingDelegate();
            }
        }

        private void InvokePropertyChangedDelegates(string? propertyName)
        {
            if (propertyName == null)
            {
                return;
            }

            if (!_propertyChangedDelegates.ContainsKey(propertyName))
            {
                return;
            }

            foreach (var propertyChangedDelegate in _propertyChangedDelegates[propertyName])
            {
                propertyChangedDelegate();
            }
        }

        public virtual void AddPropertyChangingDeletegate(string propertyName, Action propertyChangingDelegate)
        {
            if (!_propertyChangingDelegates.ContainsKey(propertyName))
            {
                _propertyChangingDelegates.Add(propertyName, new());
            }

            _propertyChangingDelegates[propertyName].Add(propertyChangingDelegate);
        }

        public virtual void RemoveAllPropertyChangingDelegates()
        {
            _propertyChangingDelegates.Clear();
        }

        public virtual void AddPropertyChangedDeletegate(string propertyName, Action propertyChangeDelegate)
        {
            if (!_propertyChangedDelegates.ContainsKey(propertyName))
            {
                _propertyChangedDelegates.Add(propertyName, new());
            }

            _propertyChangedDelegates[propertyName].Add(propertyChangeDelegate);
        }

        public virtual void RemoveAllPropertyChangedDelegates()
        {
            _propertyChangedDelegates.Clear();
        }

        protected virtual void OnPropertyChanging([CallerMemberName] string? propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (propertyName == null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return;
            }

            if (propertyName != nameof(HasChanged) && IsPropertyChangeMonitoringEnabled && !_ignoredPropertyNames.Contains(propertyName))
            {
                HasChanged = true;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void IgnorePropertyMonitoringFor(string propertyName)
        {
            _ignoredPropertyNames.Add(propertyName);
        }

        /// <summary>
        /// Forces SetProperty on all properties
        /// </summary>
        public void SetAllProperties()
        {
            var hasChanged = HasChanged;

            foreach (var property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty))
            {
                if (property.CanWrite)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property.Name));
                }
            }

            HasChanged = hasChanged;
        }
    }
}
