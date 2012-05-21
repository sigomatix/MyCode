using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestInitTestCleanup
{
    /// <summary>
    /// Description résumée pour UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        private bool initDone = false;
        private bool cleanDone = true;

        [TestInitialize]
        public void TestInit()
        {
            initDone = true;
        }

        [TestMethod]
        public void TestMethod1()
        {
            if (!cleanDone) Assert.Fail("Clean should have been called");
            if (!initDone) Assert.Fail("Init should have been called");
            Console.Write("TestMethod1");
            cleanDone = false;
            initDone = false;
        }

        [TestMethod]
        public void TestMethod2()
        {
            if (!cleanDone) Assert.Fail("Clean should have been called");
            if (!initDone) Assert.Fail("Init should have been called");
            Console.Write("TestMethod2");
            cleanDone = false;
            initDone = false;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            cleanDone = true;
        }
    }
}
