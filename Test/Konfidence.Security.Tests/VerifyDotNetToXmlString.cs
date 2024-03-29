﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

namespace Konfidence.Security.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class VerifyDotNetToXmlString
    {
        [TestMethod]
        public void When_Example_Executed_Should_work()
        {
            // Create a key and save it in a container.  
            GenKey_SaveInContainer("MyKeyContainer");

            // Retrieve the key from the container.  
            GetKeyFromContainer("MyKeyContainer");

            // Delete the key from the container.  
            DeleteKeyFromContainer("MyKeyContainer");

            // Create a key and save it in a container.  
            GenKey_SaveInContainer("MyKeyContainer");

            // Delete the key from the container.  
            DeleteKeyFromContainer("MyKeyContainer");
        }

        public static void GenKey_SaveInContainer(string ContainerName)
        {
            // Create the CspParameters object and set the key container   
            // name used to store the RSA key pair.  
            var cp = new CspParameters {KeyContainerName = ContainerName};

            // Create a new instance of RSACryptoServiceProvider that accesses  
            // the key container MyKeyContainerName.  
            var rsa = new RSACryptoServiceProvider(cp);

            // Display the key information to the console.  
            Debug.WriteLine("Key added to container: \n  {0}", rsa.ToXmlString(true));
        }

        public static void GetKeyFromContainer(string ContainerName)
        {
            // Create the CspParameters object and set the key container   
            // name used to store the RSA key pair.  
            var cp = new CspParameters {KeyContainerName = ContainerName};

            // Create a new instance of RSACryptoServiceProvider that accesses  
            // the key container MyKeyContainerName.  
            var rsa = new RSACryptoServiceProvider(cp);

            // Display the key information to the console.  
            Debug.WriteLine("Key retrieved from container : \n {0}", rsa.ToXmlString(true));
        }

        public static void DeleteKeyFromContainer(string ContainerName)
        {
            // Create the CspParameters object and set the key container   
            // name used to store the RSA key pair.  
            var cp = new CspParameters {KeyContainerName = ContainerName};

            // Create a new instance of RSACryptoServiceProvider that accesses  
            // the key container.  
            var rsa = new RSACryptoServiceProvider(cp)
            {
                // Delete the key entry in the container.  
                PersistKeyInCsp = false
            };

            // Call Clear to release resources and delete the key from the container.  
            rsa.Clear();

            Debug.WriteLine("Key deleted.");
        }
    }
}
