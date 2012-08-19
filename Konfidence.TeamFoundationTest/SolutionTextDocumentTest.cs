using Konfidence.TeamFoundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TeamFoundationTest
{
    
    
    /// <summary>
    ///This is a test class for SolutionTextDocumentTest and is intended
    ///to contain all SolutionTextDocumentTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SolutionTextDocumentTest
    {

        #region test context
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
        #endregion test context

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for AddProjectFile
        ///</summary>
        [TestMethod()]
        public void AddProjectFileTest()
        {
            // move projectfile to subdir
            string testProjectDir =TestContext.TestDeploymentDir +  @"\TestClassGeneratorClassesDb";
            Directory.CreateDirectory(testProjectDir);
            string testProjectFile = testProjectDir + @"\TestClassGeneratorClasses.csproj";

            File.Move(TestContext.TestDeploymentDir + @"\TestClassGeneratorClasses.csproj", testProjectFile);

            SolutionTextDocument target = SolutionTextDocument.GetSolutionXmlDocument(TestContext.TestDeploymentDir + @"\KonfidenceClassGenerator.sln"); // TODO: Initialize to an appropriate value

            ProjectXmlDocument projectFile = ProjectXmlDocument.GetProjectXmlDocument(testProjectFile);

            target.AddProjectFile(projectFile);

            target.Save();

            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
