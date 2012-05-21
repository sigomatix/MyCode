using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssemblyInitAssemblyCleanup
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            if (UnitTest1.initCount != 1) Assert.Fail("AssemblyInit should have been called once");
        }

        [TestMethod]
        public void TestMethod2()
        {
            if (UnitTest1.initCount != 1) Assert.Fail("AssemblyInit should have been called once");
        }

        [AssemblyCleanup]
        public static void TestCleanup()
        {
        }
    }
}
