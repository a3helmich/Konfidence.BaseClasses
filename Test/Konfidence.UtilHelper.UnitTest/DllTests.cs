using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.UtilHelper.UnitTest;

[TestClass]
public class DllTests
{
    [TestMethod]
    public void LoadWin32Library_WithRealSystemDll_ReturnsHandleThatResolvesAnExportedFunction()
    {
        // Arrange
        const string dllFilePath = "kernel32.dll";

        // Act
        IntPtr moduleHandle = Dll.LoadWin32Library(dllFilePath);

        try
        {
            IntPtr functionPointer = Dll.GetProcAddress(moduleHandle, "GetCurrentProcessId");

            // Assert
            moduleHandle.Should().NotBe(IntPtr.Zero);
            functionPointer.Should().NotBe(IntPtr.Zero);
        }
        finally
        {
            Dll.FreeLibrary(moduleHandle);
        }
    }

    [TestMethod]
    public void LoadWin32Library_WithNonExistentDll_ThrowsApplicationException()
    {
        // Arrange
        string nonExistentDllFilePath = $"DoesNotExist_{Guid.NewGuid():N}.dll";

        // Act
        Action action = () => Dll.LoadWin32Library(nonExistentDllFilePath);

        // Assert
        action.Should().Throw<ApplicationException>();
    }
}
