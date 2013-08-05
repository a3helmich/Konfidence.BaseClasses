using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseDataBaseClassesTest
{
    
    
    /// <summary>
    ///This is a test class for DataBaseStructureTest and is intended
    ///to contain all DataBaseStructureTest Unit Tests
    ///</summary>
    [TestClass]
    public class DataBaseStructureTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

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
        ///A test for BuildStructure
        ///</summary>
        [TestMethod, TestCategory("DataBaseStructure")]
        public void BuildStructureTest()
        {
            //DataBaseStructure target = new DataBaseStructure(); // TODO: Initialize to an appropriate value

            //target.BuildStructure();

            //Assert.AreEqual(12, target.TableList.Count); // newsletter heeft nu 12 tabellen
        }
    }
}
