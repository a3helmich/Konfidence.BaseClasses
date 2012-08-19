using Konfidence.TeamFoundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Konfidence.BaseUnitTestClasses;

namespace TeamFoundationTest
{
    
    
    /// <summary>
    ///This is a test class for SolutionTextDocumentTest and is intended
    ///to contain all SolutionTextDocumentTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SolutionTextDocumentTest
    {
        private static bool _InitDone = false;

        static private string _TestDir = @"\TestClassGeneratorClassesDb";
        static private string _ExistingDir = @"\DataItemGenerator";
        static private string _TestProject = @"\TestClassGeneratorClasses.csproj";
        static private string _ExistingProject = @"\DataItemGenerator.csproj";
        static private string _StartSolution = @"\KonfidenceClassGenerator_Start.sln";
        static private string _TestSolution = @"\KonfidenceClassGenerator.sln";
        static private string _TestSolutionResult = @"\KonfidenceClassGenerator_TestResult.sln";

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

        public string TestProjectDir
        {
            get { return TestContext.TestDeploymentDir + _TestDir; }
        }

        public string TestProjectFile
        {
            get { return TestProjectDir + _TestProject; }
        }

        public string ExistingProjectDir
        {
            get { return TestContext.TestDeploymentDir + _ExistingDir; }
        }

        public string ExistingProjectFile
        {
            get
            {
                return ExistingProjectDir + _ExistingProject;
            }
        }

        public string TestSolutionFile
        {
            get
            {
                return TestContext.TestDeploymentDir + _TestSolution;
            }
        }

        public string TestSolutionResultFile
        {
            get
            {
                return TestContext.TestDeploymentDir + _TestSolutionResult;
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
        [TestInitialize()]
        public void MyTestInitialize()
        {
            if (!_InitDone)
            {
                // move projectfile to subdir
                Directory.CreateDirectory(TestProjectDir);
                Directory.CreateDirectory(ExistingProjectDir);

                File.Copy(TestContext.TestDeploymentDir + _TestProject, TestProjectFile, true);
                File.Copy(TestContext.TestDeploymentDir + _ExistingProject, ExistingProjectFile, true);

                _InitDone = true;
            }

            File.Copy(TestContext.TestDeploymentDir + _StartSolution, TestSolutionFile, true);
        }
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
            SolutionTextDocument target = SolutionTextDocument.GetSolutionXmlDocument(TestSolutionFile); 

            ProjectXmlDocument projectFile = ProjectXmlDocument.GetProjectXmlDocument(TestProjectFile);

            target.AddProjectFile(projectFile);

            target.Save();

            Assert.IsTrue(BaseFileTest.TextFileEqual(TestSolutionFile, TestSolutionResultFile), "Solutionfiles zijn niet gelijk.");
        }

        /// <summary>
        ///A test for ParseFile
        ///</summary>
        [TestCategory("SolutionTextDocument"), TestMethod()]
        [DeploymentItem("Konfidence.TeamFoundation.dll")]
        public void ParseFileTest()
        {
            SolutionTextDocument_Accessor target = SolutionTextDocument_Accessor.AttachShadow(SolutionTextDocument_Accessor.GetSolutionXmlDocument(TestSolutionFile)); 

            // gebeurt impliciet als het bestand geladen wordt:  target.ParseSolutionFile();


            Assert.AreEqual(7, target.SccNumberOfProjects, "1.Het aantal projecten moet 7 zijn!");
            Assert.AreEqual(7, target.NumberOfSolutionProjects, "2.Het aantal projecten moet 7 zijn!");

            ProjectXmlDocument projectFile = ProjectXmlDocument.GetProjectXmlDocument(ExistingProjectFile);

            target.AddProjectFile(projectFile);

            Assert.AreEqual(7, target.SccNumberOfProjects, "3.Het aantal projecten moet 7 zijn!");
            Assert.AreEqual(7, target.NumberOfSolutionProjects, "4.Het aantal projecten moet 7 zijn!");

            projectFile = ProjectXmlDocument.GetProjectXmlDocument(TestProjectFile);

            target.AddProjectFile(projectFile);

            Assert.AreEqual(8, target.SccNumberOfProjects, "5.Het aantal projecten moet 8 zijn!");
            Assert.AreEqual(8, target.NumberOfSolutionProjects, "6.Het aantal projecten moet 8 zijn!");
        }
    }
}
