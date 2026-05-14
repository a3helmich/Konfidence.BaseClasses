using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Konfidence.Base.Wpf;

public class BaseViewModel : INotifyPropertyChanged
{
    private int _suppressNotifications;

    public event PropertyChangedEventHandler? PropertyChanged;

    [UsedImplicitly]
    protected bool IsNotificationSuppressed => _suppressNotifications > 0;

    [UsedImplicitly]
    protected IDisposable SuppressNotifications()
    {
        _suppressNotifications++;

        return new NotificationScope(this);
    }

    private void ResumeNotifications()
    {
        _suppressNotifications--;
    }

    [UsedImplicitly]
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (IsNotificationSuppressed)
        {
            return;
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    [UsedImplicitly]
    public bool SetField<T>(
        ref T field,
        T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;

        OnPropertyChanged(propertyName);

        return true;
    }

    [UsedImplicitly]
    public bool SetFrozenField<T>(
        ref T field,
        T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        OnPropertyChanged(propertyName);

        return true;
    }

    private sealed class NotificationScope : IDisposable
    {
        private readonly BaseViewModel _owner;

        public NotificationScope(BaseViewModel owner) => _owner = owner;

        public void Dispose() => _owner.ResumeNotifications();
    }
}
