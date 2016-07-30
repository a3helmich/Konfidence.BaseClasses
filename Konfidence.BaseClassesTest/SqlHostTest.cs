using Konfidence.BaseData;
using Konfidence.BaseData.SqlDbSchema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.TestBaseClasses
{
    [TestClass]
    public class SqlHostTest
    {
        [TestMethod, TestCategory("SqlServer")]
        [ExpectedException(typeof(System.Data.SqlClient.SqlException))]
        //[ExpectedException(typeof(SqlHostException))]
        public void SqlServerNotFound()
        {
            var tableDataItemList = new TableDataItemList("TestDatabase");  // does not exist
        }

        [TestMethod, TestCategory("SqlServer")]
        public void SqlServerExists()
        {
            DataBaseStructure target = new DataBaseStructure("Newsletter"); // TODO: Initialize to an appropriate value

            target.BuildStructure();

            Assert.AreEqual(25, target.TableList.Count); // newsletter heeft nu 25 tabellen

            //var tableDataItemList = new TableDataItemList("Newsletter");   // should exist
        }
    }
}
