using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.Security.UnitTest;

[TestClass]
[ExcludeFromCodeCoverage]
[SupportedOSPlatform("windows")]
public class VerifyDotNetToXmlString
{
    [TestMethod]
    public void KeyContainer_CreateRetrieveAndDelete_Succeeds()
    {
        if (!OperatingSystem.IsWindows())
        {
            Assert.Inconclusive("CSP key containers are only supported on Windows.");
            return;
        }

        string containerName = $"KonfidenceSecurityTest_{Guid.NewGuid():N}";

        try
        {
            GenKey_SaveInContainer(containerName);
            GetKeyFromContainer(containerName);
            DeleteKeyFromContainer(containerName);

            GenKey_SaveInContainer(containerName);
            DeleteKeyFromContainer(containerName);
        }
        finally
        {
            DeleteKeyFromContainer(containerName);
        }
    }

    private static void GenKey_SaveInContainer(string containerName)
    {
        CspParameters cspParameters = new() { KeyContainerName = containerName };

        using RSACryptoServiceProvider rsa = new(cspParameters);

        Debug.WriteLine("Key added to container: {0}", containerName);
    }

    private static void GetKeyFromContainer(string containerName)
    {
        CspParameters cspParameters = new() { KeyContainerName = containerName };

        using RSACryptoServiceProvider rsa = new(cspParameters);

        Debug.WriteLine("Key retrieved from container: {0}", containerName);
    }

    private static void DeleteKeyFromContainer(string containerName)
    {
        CspParameters cspParameters = new() { KeyContainerName = containerName };

        using RSACryptoServiceProvider rsa = new(cspParameters)
        {
            PersistKeyInCsp = false
        };

        rsa.Clear();

        Debug.WriteLine("Key deleted from container: {0}", containerName);
    }
}
