using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;
using PMSTest.Proxy;
using System.Runtime.InteropServices;
using Moq;

/*
 * 
 * DistributedRunner distribute test methods on runners -> TestRunner schedule methods for running in proxy (including init/cleanup) -> Proxy in AppDomain execute the given methods
 * 
 * */

namespace PMSTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ItShouldDistributeTheTestMethodsTestsEquallyAmongstTheRunners()
        {
            var assemblyResolver = new Mock<IAssemblyResolver>();
            var testAssembly = new Mock<IAssembly>();
            var testClassType = new Mock<IType>();
            var methods = Enumerable.Range(1, 20).Select(i =>
                {
                    var m = new Mock<IMethodInfo>();
                    if (i < 8)
                    {
                        m.Setup(a => a.GetCustomAttributes(typeof(TestMethodAttribute), true)).Returns(new Object[] { new Object() });
                    }
                    else
                    {
                        m.Setup(a => a.GetCustomAttributes(typeof(TestMethodAttribute), true)).Returns(new Object[] { });
                    }
                    m.Setup(n => n.Name).Returns("Method " + i);
                    return m;
                }).ToList();
            var testRunners = Enumerable.Range(1, 3).Select(i => new TestRunnerStub()).ToList();

            assemblyResolver.Setup(a => a.LoadFrom(@"c:\someAssembly.dll")).Returns(testAssembly.Object);
            testAssembly.Setup(t => t.GetTypes()).Returns(new IType[] { testClassType.Object });
            testClassType.Setup(t => t.GetMethods()).Returns(methods.Select(m => m.Object).ToArray());

            var runner = new DistributedRunner(testRunners, assemblyResolver.Object);

            var runTask = runner.Run(@"c:\someAssembly.dll");
            runTask.Wait();

            Assert.IsTrue(new[] { 3, 2 }.Contains(testRunners[0].ExecutedMethods.Count()));
            Assert.IsTrue(new[] { 3, 2 }.Contains(testRunners[1].ExecutedMethods.Count()));
            Assert.IsTrue(new[] { 3, 2 }.Contains(testRunners[2].ExecutedMethods.Count()));

            Assert.AreEqual(7, testRunners.Sum(t => t.ExecutedMethods.Count()));

            Enumerable.Range(1, 7).All(i =>
            {
                var found = testRunners.Any(t => t.ExecutedMethods.Contains("Method " + i));
                Assert.IsTrue(found, "cant find runner for method " + i);
                return found;
            });
        }

        [TestMethod]
        public void GivenThereIsOnlyOneTestMethodTheRunnerShouldRunnerATaskForExecutingThisOneTestMethod()
        {
            var proxyMock = new Mock<IProxy>();
            var runner = new MsTestRunner(proxyMock.Object);

            var declaringType = new Mock<IType>();
            declaringType.Setup(t => t.FullName).Returns("SomeTestClass");

            var testmethodMock = new Mock<IMethodInfo>();
            testmethodMock.Setup(m => m.GetCustomAttributes(typeof(TestMethodAttribute), true)).Returns(new object[] { new object() });
            testmethodMock.Setup(m => m.DeclaringType).Returns(declaringType.Object);
            testmethodMock.Setup(m => m.Name).Returns("SomeTestMethod");
            var methods = new IMethodInfo[] { testmethodMock.Object };

            runner.Run(methods).Wait();

            proxyMock.Verify(p => p.Run("SomeTestClass", "SomeTestMethod"), Times.Once());

        }
    }
}
