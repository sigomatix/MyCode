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
using System.Linq.Expressions;

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
            var testMethodExtractor = new Mock<ITestMethodExtractor>();
            var methods = Enumerable.Range(1, 7).Select(i => BuildMethod("SomClass", "Method " + i).Object ).ToList();
            var testRunners = Enumerable.Range(1, 3).Select(i => new TestRunnerStub()).ToList();

            testMethodExtractor.Setup(e => e.GetTestMethods()).Returns(methods);
            var runner = new DistributedRunner(testRunners, testMethodExtractor.Object);

            var runTask = runner.Run();
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
        public void GivenThereIsOnlyOneTestMethodTheRunnerShouldRunATaskForExecutingThisOneTestMethodThroughtItsProxy()
        {
            var proxyMock = new Mock<IProxy>();
            var extractor = new Mock<ITestMethodExtractor>();
            var runner = new MsTestRunner(proxyMock.Object, extractor.Object);

            var testmethodMock = BuildMethod("SomeTestClass", "SomeTestMethod");
            var methods = new IMethodInfo[] { testmethodMock.Object };

            extractor.Setup(e => e.GetTestMethods()).Returns(methods);

            runner.Run(methods).Wait();

            proxyMock.Verify(p => p.Run("SomeTestClass", "SomeTestMethod"), Times.Once());
        }

        [TestMethod]
        public void GivenThereAreTwotMethodsTheRunnerShouldRunnerATaskForExecutingTheseTwoTestMethodThroughtItsProxy()
        {
            var proxyMock = new Mock<IProxy>();
            var extractor = new Mock<ITestMethodExtractor>();
            var runner = new MsTestRunner(proxyMock.Object, extractor.Object);

            var testmethodMock1 = BuildMethod("SomeTestClass", "SomeTestMethod1");
            var testmethodMock2 = BuildMethod("SomeTestClass", "SomeTestMethod2");

            var methods = new IMethodInfo[] { testmethodMock1.Object, testmethodMock2.Object };

            runner.Run(methods).Wait();

            proxyMock.Verify(p => p.Run("SomeTestClass", "SomeTestMethod1"), Times.Once());
            proxyMock.Verify(p => p.Run("SomeTestClass", "SomeTestMethod2"), Times.Once());
            proxyMock.Verify(p => p.Run(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void ItShouldRunTheAsemblyInitialiseOnceAndBeforeEverythingElseIfPresent()
        {
            var proxy = new ProxyStub();
            var extractor = new Mock<ITestMethodExtractor>();
            var runner = new MsTestRunner(proxy, extractor.Object);

            var assemblyInit = BuildMethod("SomeTestClass", "AssemblyInit");
            var testmethodMock1 = BuildMethod("SomeTestClass", "SomeTestMethod1");
            var testmethodMock2 = BuildMethod("SomeTestClass", "SomeTestMethod2");

            var methods = new IMethodInfo[] { testmethodMock1.Object, testmethodMock2.Object };

            extractor.Setup(e => e.GetTestMethods()).Returns(methods);
            extractor.Setup(e => e.GetAssemblyInitialise()).Returns(assemblyInit.Object);

            runner.Run(methods).Wait();

            Assert.AreEqual(3, proxy.Log.Count);
            Assert.IsTrue(proxy.Log[0].Type == "SomeTestClass" && proxy.Log[0].Method == "AssemblyInit");
            Assert.IsTrue(proxy.Log[1].Type == "SomeTestClass" && proxy.Log[1].Method == "SomeTestMethod1");
            Assert.IsTrue(proxy.Log[2].Type == "SomeTestClass" && proxy.Log[2].Method == "SomeTestMethod2");
        }

        [TestMethod]
        public void ItShouldRunTheAsemblyCleanupOnceAndAfterEverythingElseIfPresent()
        {
            var proxy = new ProxyStub();
            var extractor = new Mock<ITestMethodExtractor>();
            var runner = new MsTestRunner(proxy, extractor.Object);

            var assemblyCleanup = BuildMethod("SomeTestClass", "AssemblyCleanup");
            var testmethodMock1 = BuildMethod("SomeTestClass", "SomeTestMethod1");
            var testmethodMock2 = BuildMethod("SomeTestClass", "SomeTestMethod2");

            var methods = new IMethodInfo[] { testmethodMock1.Object, testmethodMock2.Object };

            extractor.Setup(e => e.GetTestMethods()).Returns(methods);
            extractor.Setup(e => e.GetAssemblyCleanup()).Returns(assemblyCleanup.Object);

            runner.Run(methods).Wait();

            Assert.AreEqual(3, proxy.Log.Count);
            Assert.IsTrue(proxy.Log[0].Type == "SomeTestClass" && proxy.Log[0].Method == "SomeTestMethod1");
            Assert.IsTrue(proxy.Log[1].Type == "SomeTestClass" && proxy.Log[1].Method == "SomeTestMethod2");
            Assert.IsTrue(proxy.Log[2].Type == "SomeTestClass" && proxy.Log[2].Method == "AssemblyCleanup");
        }

        [TestMethod]
        public void ItShouldRunTestInitializeOfTheTestClassBeforeEachTestMethodOfTheTestClass()
        {
            var proxy = new ProxyStub();
            var extractor = new Mock<ITestMethodExtractor>();
            var runner = new MsTestRunner(proxy, extractor.Object);

            var testInitialize = BuildMethod("SomeTestClass", "TestInitialize");
            var testmethodMock1 = BuildMethod("SomeTestClass", "SomeTestMethod1");
            var testmethodMock2 = BuildMethod("SomeTestClass", "SomeTestMethod2");

            var methods = new IMethodInfo[] { testmethodMock1.Object, testmethodMock2.Object };

            extractor.Setup(e => e.GetTestMethods()).Returns(methods);
            extractor.Setup(e => e.GetTestInitialize(testmethodMock1.Object)).Returns(testInitialize.Object);
            extractor.Setup(e => e.GetTestInitialize(testmethodMock2.Object)).Returns(testInitialize.Object);

            runner.Run(methods).Wait();

            Assert.AreEqual(4, proxy.Log.Count);
            Assert.IsTrue(proxy.Log[0].Type == "SomeTestClass" && proxy.Log[0].Method == "TestInitialize");
            Assert.IsTrue(proxy.Log[1].Type == "SomeTestClass" && proxy.Log[1].Method == "SomeTestMethod1");
            Assert.IsTrue(proxy.Log[2].Type == "SomeTestClass" && proxy.Log[2].Method == "TestInitialize");
            Assert.IsTrue(proxy.Log[3].Type == "SomeTestClass" && proxy.Log[3].Method == "SomeTestMethod2");
        }

        [TestMethod]
        public void ItShouldRunTestCleanupOfTheTestClassAfterEachTestMethodOfTheTestClass()
        {
            var proxy = new ProxyStub();
            var extractor = new Mock<ITestMethodExtractor>();
            var runner = new MsTestRunner(proxy, extractor.Object);

            var testCleanup = BuildMethod("SomeTestClass", "TestCleanup");
            var testmethodMock1 = BuildMethod("SomeTestClass", "SomeTestMethod1");
            var testmethodMock2 = BuildMethod("SomeTestClass", "SomeTestMethod2");

            var methods = new IMethodInfo[] { testmethodMock1.Object, testmethodMock2.Object };

            extractor.Setup(e => e.GetTestMethods()).Returns(methods);
            extractor.Setup(e => e.GetTestCleanup(testmethodMock1.Object)).Returns(testCleanup.Object);
            extractor.Setup(e => e.GetTestCleanup(testmethodMock2.Object)).Returns(testCleanup.Object);

            runner.Run(methods).Wait();

            Assert.AreEqual(4, proxy.Log.Count);
            Assert.IsTrue(proxy.Log[0].Type == "SomeTestClass" && proxy.Log[0].Method == "SomeTestMethod1");
            Assert.IsTrue(proxy.Log[1].Type == "SomeTestClass" && proxy.Log[1].Method == "TestCleanup");
            Assert.IsTrue(proxy.Log[2].Type == "SomeTestClass" && proxy.Log[2].Method == "SomeTestMethod2");
            Assert.IsTrue(proxy.Log[3].Type == "SomeTestClass" && proxy.Log[3].Method == "TestCleanup");
        }

        private static Mock<IMethodInfo> BuildMethod(string className, string methodName)
        {
            var testmethodMock = new Mock<IMethodInfo>();
            var dt = new Mock<IType>();
            testmethodMock.Setup(m => m.Name).Returns(methodName);
            testmethodMock.Setup(m => m.DeclaringType).Returns(dt.Object);
            dt.Setup(t => t.FullName).Returns(className);
            return testmethodMock;
        }
    }
}
