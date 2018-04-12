using System;
using Konfidence.BaseDatabaseClasses.Tests.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Konfidence.BaseDatabaseClasses.Tests
{
    [TestClass]
    public class BaseDataItemTest
    {
        [TestMethod]
        public void BaseDataItemWhenInstatiatedShouldSelectClient()
        {
            var x = "my string";

            var dataItem = new DataItem();
        }
    }
}
