using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Noesis.Javascript;
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
        [DeploymentItem(@"tests\AdditionTests.js")]
        [DeploymentItem(@"tests\DivideTests.js")]
        public void ShouldListProperlyAllTestMethods()
        {
            var jsTestGenerator = new JSTestGenerator();

            jsTestGenerator.AddTestFile("AdditionTests.js");
            jsTestGenerator.AddTestFile("DivideTests.js");

            var testMethods = jsTestGenerator.GetAllTestMethods();

            Assert.AreEqual("TestAddditionShouldAddPositives", testMethods[0]);
            Assert.AreEqual("TestAddditionShouldAddNegatives", testMethods[1]);
            Assert.AreEqual("TestAddditionShouldAddNegativesAndPositiv", testMethods[2]);
            Assert.AreEqual("TestDivideBySameShouldReturnOne", testMethods[3]);
            Assert.AreEqual("TestDivideByZeroShouldReturnInfinity", testMethods[4]);
            Assert.AreEqual(5, testMethods.Count);
        }

        [TestMethod]
        [DeploymentItem(@"libs\Assert.js")]
        [DeploymentItem(@"tests\AdditionTests.js")]
        [DeploymentItem(@"tests\DivideTests.js")]
        public void ShouldGenerateCodeWhenNoIncludesProvided()
        {
            var jsTestGenerator = new JSTestGenerator();

            jsTestGenerator.AddIncludeFile("Assert.js");
            jsTestGenerator.AddTestFile("AdditionTests.js");
            jsTestGenerator.AddTestFile("DivideTests.js");

            /* TODO optimize to run only the test code that includes the test method */
            var expectedCode =
@"using(var context = new Noesis.Javascript.JavascriptContext())
{
    context.Run(File.ReadAllText(@""Assert.js""));
    context.Run(File.ReadAllText(@""AdditionTests.js""));
    context.Run(File.ReadAllText(@""DivideTests.js""));
    context.Run(""TestAddditionShouldAddPositives();"");
}";

            var actualCode = jsTestGenerator.GenerateTestCodeForMethod("TestAddditionShouldAddPositives");

            Assert.AreEqual(expectedCode, actualCode);
        }
    }
}
