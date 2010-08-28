using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using Konfidence.BaseData;
using Konfidence.BaseData.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace Konfidence.TestBaseClasses
{
    [TestClass]
    public class SqlHostTest
    {
        [TestMethod]
        public void SqlServerExists()
        {
            TableDataItemList tableList = null;

            try
            {
                tableList = new TableDataItemList();
            }
            catch (SqlHostException)
            {
                // expected
            }
            catch (SqlException)
            {
                // unexpected
                throw;
            }
            catch (Exception)
            {
                // totally unexpected
                throw;
            }

            Assert.IsNotNull(tableList);
        }
    }
}
