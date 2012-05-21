using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssemblyInitAssemblyCleanup
{
    /// <summary>
    /// Description résumée pour UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public static int initCount = 0;

        [AssemblyInitialize]
        public static void TestInit(TestContext context)
        {
            initCount++;
        }

        [TestMethod]
        public void TestMethod1()
        {
            if (initCount != 1) Assert.Fail("AssemblyInit should have been called once");
        }

        [TestMethod]
        public void TestMethod2()
        {
            if (initCount != 1) Assert.Fail("AssemblyInit should have been called once");
        }
    }
}
