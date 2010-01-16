using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Konfidence.TeamFoundation;
using System.IO;

namespace Konfidence.TestBaseClasses
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TeamFoundation
    {
        public TeamFoundation()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestGetGlobalPermissions()
        {
            TfsPermissions tfsPermissions = new TfsPermissions("tfs.konfidence.nl");

            List<string> globalPermissions = tfsPermissions.GetGlobalPermissions();
        }

        [TestMethod]
        public void TestGetItemPermissions()
        {
            TfsPermissions tfsPermissions = new TfsPermissions("tfs.konfidence.nl");

            List<string> itemPermissions = tfsPermissions.GetItemPermissions("$/Konfidence/BaseClasses");
        }

        [TestMethod]
        public void TestCheckOut()
        {
            TfsPermissions tfsPermissions = new TfsPermissions("tfs.konfidence.nl");

            string testFile = @"C:\Projects\Konfidence\BaseClasses\Konfidence.TestBaseClasses\TestData\CheckOutClass.cs";

            if (tfsPermissions.CheckOut(testFile))
            {
                tfsPermissions.Undo(testFile);
            }
            else
            {
                throw new Exception("Bestand kan niet worden uitgechecked!");
            }
        }

        [TestMethod]
        public void TestReferenceRebaser()
        {
            ReferenceReBaser reBaser = new ReferenceReBaser();

            string basePath = @"C:\Projects\Konfidence\BaseClasses\";

            reBaser.ReBaseProjects(basePath);
        }
    }
}
