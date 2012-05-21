using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace TwoTestMethodsInSameTestClass
{
    /// <summary>
    /// Description résumée pour UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine("TestMethod1");
            Thread.Sleep(5000);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Console.WriteLine("TestMethod2");
            Thread.Sleep(5000);
        }
    }
}
