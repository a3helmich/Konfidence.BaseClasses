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

    private sealed class TestViewModel : BaseViewModel
    {
        public System.Collections.Generic.List<string> ChangedProperties { get; } = [];

        public string Name { get; private set; } = string.Empty;

        public TestViewModel()
        {
            PropertyChanged += OnPropertyChanged;
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
