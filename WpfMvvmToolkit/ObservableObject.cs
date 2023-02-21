using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace WpfMvvmToolkit
{
    public class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private readonly Dictionary<string, List<Action>> _propertyChangedDelegates = new();
        private bool _hasChanged;

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsPropertyChangeMonitoringEnabled { get; set; } = true;
        public bool HasChanged
        {
            get => _hasChanged;
            set => SetProperty(ref _hasChanged, value);
        }

        protected bool SetProperty<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);
            field = newValue;
            OnPropertyChanged(propertyName);
            InvokePropertyChangedDelegates(propertyName);
            return true;
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

        public void AddPropertyChangedDeletegate(string propertyName, Action propertyChangeDelegate)
        {
            if (!_propertyChangedDelegates.ContainsKey(propertyName))
            {
                _propertyChangedDelegates.Add(propertyName, new());
            }

            _propertyChangedDelegates[propertyName].Add(propertyChangeDelegate);
        }

        public void RemoveAllPropertyChangedDelegates()
        {
            _propertyChangedDelegates.Clear();
        }

        protected virtual void OnPropertyChanging([CallerMemberName] string? propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName != nameof(HasChanged) && IsPropertyChangeMonitoringEnabled)
            {
                HasChanged = true;
            }
        }

    }
}
