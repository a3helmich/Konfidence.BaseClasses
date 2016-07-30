using Konfidence.BaseData.SqlDbSchema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.TestBaseClasses
{
    [TestClass]
    public class SqlHostTest
    {
        [TestMethod, TestCategory("SqlServer")]
        [ExpectedException(typeof(System.Data.SqlClient.SqlException))]
        public void SqlServerNotFound()
        {
            var tableDataItemList = new TableDataItemList("TestDatabase");  // does not exist
        }

        [TestMethod, TestCategory("SqlServer")]
        public void SqlServerExists()
        {
            var tableDataItemList = new TableDataItemList("Newsletter");   // should exist
        }
    }
}
