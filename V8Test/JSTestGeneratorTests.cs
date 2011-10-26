using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace V8Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class JSTestGeneratorTests
    {
       
        [TestMethod]
        public void ShouldListProperlyAllTestMethods()
        {
            var jsTestGenerator = new JSTestGenerator();

            jsTestGenerator.AddTestFile(@"tests\AdditionTests.js");
            jsTestGenerator.AddTestFile(@"tests\DivideTests.js");
            jsTestGenerator.SetTestFilesRelocationPath("tests");

            var testMethods = jsTestGenerator.GetAllTestMethods();

            Assert.AreEqual("TestAddditionShouldAddPositives", testMethods[0]);
            Assert.AreEqual("TestAddditionShouldAddNegatives", testMethods[1]);
            Assert.AreEqual("TestAddditionShouldAddNegativesAndPositiv", testMethods[2]);
            Assert.AreEqual("TestDivideBySameShouldReturnOne", testMethods[3]);
            Assert.AreEqual("TestDivideByZeroShouldReturnInfinity", testMethods[4]);
            Assert.AreEqual(5, testMethods.Count);
        }

        [TestMethod]
        public void ShouldListProperlyAllTestMethods2()
        {
            var jsTestGenerator = new JSTestGenerator();

            jsTestGenerator.AddTestFile(@"tests\AdditionTests.js");
            jsTestGenerator.AddTestFile(@"tests\DivideTests.js");
            jsTestGenerator.SetTestFilesRelocationPath("tests");

            var testMethods = jsTestGenerator.GetAllTestMethods();

            Assert.AreEqual("TestAddditionShouldAddPositives", testMethods[0]);
            Assert.AreEqual("TestAddditionShouldAddNegatives", testMethods[1]);
            Assert.AreEqual("TestAddditionShouldAddNegativesAndPositiv", testMethods[2]);
            Assert.AreEqual("TestDivideBySameShouldReturnOne", testMethods[3]);
            Assert.AreEqual("TestDivideByZeroShouldReturnInfinity", testMethods[4]);
            Assert.AreEqual(5, testMethods.Count);
        }

        [TestMethod]
        public void ShouldListProperlyAllTestMethods3()
        {
            var jsTestGenerator = new JSTestGenerator();

            jsTestGenerator.AddTestFile(@"tests\AdditionTests.js");
            jsTestGenerator.AddTestFile(@"tests\DivideTests.js");
            jsTestGenerator.SetTestFilesRelocationPath("tests");

            var testMethods = jsTestGenerator.GetAllTestMethods();

            Assert.AreEqual("TestAddditionShouldAddPositives", testMethods[0]);
            Assert.AreEqual("TestAddditionShouldAddNegatives", testMethods[1]);
            Assert.AreEqual("TestAddditionShouldAddNegativesAndPositiv", testMethods[2]);
            Assert.AreEqual("TestDivideBySameShouldReturnOne", testMethods[3]);
            Assert.AreEqual("TestDivideByZeroShouldReturnInfinity", testMethods[4]);
            Assert.AreEqual(5, testMethods.Count);
        }

        [TestMethod]
        public void ShouldGenerateCodeWhenNoIncludesProvided()
        {
            var jsTestGenerator = new JSTestGenerator();

            jsTestGenerator.AddIncludeFile(@"libs\Assert.js");
            jsTestGenerator.AddTestFile(@"tests\AdditionTests.js");
            jsTestGenerator.AddTestFile(@"tests\DivideTests.js");
            jsTestGenerator.SetTestFilesRelocationPath("tests");

            /* TODO optimize to run only the test code that includes the test method */
            var expectedCode =
@"var context = new Noesis.Javascript.JavascriptContext();
{
    context.Run(File.ReadAllText(@""libs\Assert.js""));
    context.Run(@""__FILENAME__ = 'tests\AdditionTests.js';"");
    context.Run(File.ReadAllText(@""tests\AdditionTests.js""));
    context.Run(""TestAddditionShouldAddPositives();"");
}";

            var actualCode = jsTestGenerator.GenerateTestCodeForMethod("TestAddditionShouldAddPositives");

            Assert.AreEqual(expectedCode, actualCode);
        }
    }
}
