using System;
using System.ComponentModel;
using FluentAssertions;
using Konfidence.Base;
using Konfidence.Base.Wpf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseClasses.UnitTest;

[TestClass]
public class BaseViewModelTests
{
    [TestMethod]
    public void SetField_DifferentValue_RaisesPropertyChangedAndReturnsTrue()
    {
        // Arrange
        TestViewModel viewModel = new();

        // Act
        bool result = viewModel.SetName("NewName");

        // Assert
        result.Should().BeTrue();
        viewModel.Name.Should().Be("NewName");
        viewModel.ChangedProperties.Should().ContainSingle().Which.Should().Be(nameof(TestViewModel.Name));
    }

    [TestMethod]
    public void SetField_SameValue_DoesNotRaisePropertyChangedAndReturnsFalse()
    {
        // Arrange
        TestViewModel viewModel = new();
        viewModel.SetName("Same");
        viewModel.ChangedProperties.Clear();

        // Act
        bool result = viewModel.SetName("Same");

        // Assert
        result.Should().BeFalse();
        viewModel.ChangedProperties.Should().BeEmpty();
    }

    [TestMethod]
    public void SetFrozenField_DifferentValue_RaisesPropertyChangedButLeavesFieldUnchanged()
    {
        // Arrange
        TestViewModel viewModel = new();

        // Act
        bool result = viewModel.SetFrozenName("Attempted");

        // Assert
        result.Should().BeTrue();
        viewModel.Name.Should().BeEmpty();
        viewModel.ChangedProperties.Should().ContainSingle().Which.Should().Be(nameof(TestViewModel.Name));
    }

    [TestMethod]
    public void SetFrozenField_SameValue_DoesNotRaisePropertyChangedAndReturnsFalse()
    {
        // Arrange
        // SetFrozenField's equality early-return was never exercised - only its
        // different-value path had a test, so the "nothing changed" contract it shares with
        // SetField was unverified.
        TestViewModel viewModel = new();

        // Act
        bool result = viewModel.SetFrozenName(string.Empty);

        // Assert
        result.Should().BeFalse();
        viewModel.Name.Should().BeEmpty();
        viewModel.ChangedProperties.Should().BeEmpty();
    }

    [TestMethod]
    public void SetField_BothValuesNull_DoesNotRaisePropertyChangedAndReturnsFalse()
    {
        // Arrange
        // EqualityComparer<T>.Default.Equals(null, null) is true, so a null-to-null assignment
        // has to count as "unchanged" rather than throwing or notifying.
        TestViewModel viewModel = new();

        // Act
        bool result = viewModel.SetNullableValue(null);

        // Assert
        result.Should().BeFalse();
        viewModel.NullableValue.Should().BeNull();
        viewModel.ChangedProperties.Should().BeEmpty();
    }

    [TestMethod]
    public void OnPropertyChanged_WithNoSubscribers_DoesNotThrow()
    {
        // Arrange
        // Every other test subscribes to PropertyChanged in the TestViewModel constructor, so the
        // null-conditional on the event invocation was never taken - a plain BaseViewModel with
        // nobody listening is the only way to reach it.
        BaseViewModel viewModel = new();

        // Act
        Action action = () => viewModel.OnPropertyChanged("SomeProperty");

        // Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void OnPropertyChanged_WithoutExplicitPropertyName_UsesCallerMemberName()
    {
        // Arrange
        // propertyName is a [CallerMemberName] optional parameter, but every existing test passes
        // it explicitly - so the compiler-supplied default was never verified.
        BaseViewModel viewModel = new();
        string? reportedPropertyName = null;
        viewModel.PropertyChanged += (_, e) => reportedPropertyName = e.PropertyName;

        // Act
        viewModel.OnPropertyChanged();

        // Assert
        reportedPropertyName.Should().Be(nameof(OnPropertyChanged_WithoutExplicitPropertyName_UsesCallerMemberName));
    }

    [TestMethod]
    public void SuppressNotifications_WhileActive_SuppressesPropertyChanged()
    {
        // Arrange
        TestViewModel viewModel = new();

        // Act
        using (viewModel.EnterSuppressedScope())
        {
            viewModel.SetName("Suppressed");
        }

        // Assert
        viewModel.Name.Should().Be("Suppressed");
        viewModel.ChangedProperties.Should().BeEmpty();
    }

    [TestMethod]
    public void SuppressNotifications_AfterScopeDisposed_ResumesPropertyChanged()
    {
        // Arrange
        TestViewModel viewModel = new();

        using (viewModel.EnterSuppressedScope())
        {
            viewModel.SetName("Suppressed");
        }

        // Act
        viewModel.SetName("Resumed");

        // Assert
        viewModel.ChangedProperties.Should().ContainSingle().Which.Should().Be(nameof(TestViewModel.Name));
    }

    [TestMethod]
    public void SuppressNotifications_NestedScopes_StaysSuppressedUntilOutermostDisposed()
    {
        // Arrange
        // Suppression is a counter, not a flag - disposing the inner scope must not resume
        // notifications while the outer scope is still open. A single-scope test cannot tell the
        // two implementations apart.
        TestViewModel viewModel = new();

        // Act
        using (viewModel.EnterSuppressedScope())
        {
            using (viewModel.EnterSuppressedScope())
            {
                viewModel.SetName("Inner");
            }

            viewModel.SetName("BetweenScopes");
        }

        viewModel.SetName("AfterAllScopes");

        // Assert
        viewModel.ChangedProperties.Should().ContainSingle().Which.Should().Be(nameof(TestViewModel.Name));
        viewModel.Name.Should().Be("AfterAllScopes");
    }

    [TestMethod]
    public void IsNotificationSuppressed_TracksScopeLifetime()
    {
        // Arrange
        TestViewModel viewModel = new();

        // Act
        bool beforeScope = viewModel.IsSuppressed;

        bool insideScope;
        using (viewModel.EnterSuppressedScope())
        {
            insideScope = viewModel.IsSuppressed;
        }

        bool afterScope = viewModel.IsSuppressed;

        // Assert
        beforeScope.Should().BeFalse();
        insideScope.Should().BeTrue();
        afterScope.Should().BeFalse();
    }

    private sealed class TestViewModel : BaseViewModel
    {
        public System.Collections.Generic.List<string> ChangedProperties { get; } = [];

        private string? _nullableValue;

        public string Name { get; private set; } = string.Empty;

        public string? NullableValue => _nullableValue;

        public bool IsSuppressed => IsNotificationSuppressed;

        public TestViewModel()
        {
            PropertyChanged += OnPropertyChanged;
        }

        public bool SetNullableValue(string? value)
        {
            return SetField(ref _nullableValue, value, nameof(NullableValue));
        }

        public bool SetName(string value)
        {
            string field = Name;
            bool changed = SetField(ref field, value, nameof(Name));
            Name = field;

            return changed;
        }

        public bool SetFrozenName(string value)
        {
            string field = Name;
            bool changed = SetFrozenField(ref field, value, nameof(Name));
            Name = field;

            return changed;
        }

        public IDisposable EnterSuppressedScope()
        {
            return SuppressNotifications();
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.IsAssigned())
            {
                ChangedProperties.Add(e.PropertyName);
            }
        }
    }
}
