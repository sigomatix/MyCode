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

    public class DataBuilder
    {
        public static Mock<IType> Type(string name)
        {
            return null;
        }

        public static Mock<IAssembly> Assembly()
        {
            return null;
        }

        public static Mock<IMethodInfo> Method(string name)
        {
            return null;
        }
    }

    public static class DataBuilderExtensions
    {

        public static Mock<IAssembly> With(this Mock<IAssembly> assembly, Mock<IType> type)
        {
            return null;
        }

        public static Mock<IType> With(this Mock<IType> type, Mock<IMethodInfo> method)
        {
            return null;
        }

        public static Mock<IMethodInfo> With(this Mock<IMethodInfo> method, Type attribute)
        {
            return null;
        }
    }

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1 : DataBuilder
    {
        [TestMethod]
        public void ItShouldDistributeTheTestMethodsTestsEquallyAmongstTheRunners()
        {
            var testMethodExtractor = new Mock<ITestMethodExtractor>();
            var methods = Enumerable.Range(1, 7).Select(i => BuildMethod("SomClass", "Method " + i).Object).ToList();
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

        [TestMethod]
        public void ShouldGetAssemblyInitialiseWhenPresent()
        {
            var assembly = new Mock<IAssembly>();
            var searcher = new Mock<IAssemblySearcher>();
            var expectedInit = BuildMethod("SomeClass", "Init");

            searcher.Setup(s => s.FindMethod(assembly.Object, typeof(AssemblyInitializeAttribute))).Returns(expectedInit.Object);

            var extractor = new MSTestExtractor(assembly.Object, searcher.Object);

            var actualInit = extractor.GetAssemblyInitialise();

            Assert.AreEqual(expectedInit.Object, actualInit);
        }

        [TestMethod]
        public void ShouldReturnNullWhenAssemnlyInitializeIsNotPresent()
        {
            var assembly = new Mock<IAssembly>();
            var searcher = new Mock<IAssemblySearcher>();
            IMethodInfo expected = null;

            searcher.Setup(s => s.FindMethod(assembly.Object, typeof(AssemblyInitializeAttribute))).Returns(expected);

            var extractor = new MSTestExtractor(assembly.Object, searcher.Object);

            var actualInit = extractor.GetAssemblyInitialise();

            Assert.AreEqual(expected, actualInit);
        }

        [TestMethod]
        public void ShouldGetAssemblyCleanupWhenPresent()
        {
            var assembly = new Mock<IAssembly>();
            var searcher = new Mock<IAssemblySearcher>();
            var expectedCleanup = BuildMethod("SomeClass", "Cleanup");

            searcher.Setup(s => s.FindMethod(assembly.Object, typeof(AssemblyCleanupAttribute))).Returns(expectedCleanup.Object);

            var extractor = new MSTestExtractor(assembly.Object, searcher.Object);

            var actualCleanup = extractor.GetAssemblyCleanup();

            Assert.AreEqual(expectedCleanup.Object, actualCleanup);
        }

        [TestMethod]
        public void ShouldReturnNullWhenAssemnlyCleanupIsNotPresent()
        {
            var assembly = new Mock<IAssembly>();
            var searcher = new Mock<IAssemblySearcher>();
            IMethodInfo expected = null;

            searcher.Setup(s => s.FindMethod(assembly.Object, typeof(AssemblyCleanupAttribute))).Returns(expected);

            var extractor = new MSTestExtractor(assembly.Object, searcher.Object);

            var actualInit = extractor.GetAssemblyInitialise();

            Assert.AreEqual(expected, actualInit);
        }

        [TestMethod]
        public void ShouldGetMethodTestInitializeWhenPresent()
        {
            var assembly = new Mock<IAssembly>();
            var testMethodOwningType = new Mock<IType>();
            var searcher = new Mock<IAssemblySearcher>();
            var expectedInit = BuildMethod("SomeClass", "Init");
            var testMethod = BuildMethod("SomeClass", "Test");
            testMethod.Setup(m => m.DeclaringType).Returns(testMethodOwningType.Object);

            searcher.Setup(s => s.FindMethod(testMethodOwningType.Object, typeof(TestInitializeAttribute))).Returns(expectedInit.Object);

            var extractor = new MSTestExtractor(assembly.Object, searcher.Object);

            var actualCleanup = extractor.GetTestInitialize(testMethod.Object);

            Assert.AreEqual(expectedInit.Object, actualCleanup);
        }

        [TestMethod]
        public void ShouldGetMethodTestCleanupWhenPresent()
        {
            var assembly = new Mock<IAssembly>();
            var testMethodOwningType = new Mock<IType>();
            var searcher = new Mock<IAssemblySearcher>();
            var expectedCleanup = BuildMethod("SomeClass", "Cleanup");
            var testMethod = BuildMethod("SomeClass", "Test");
            testMethod.Setup(m => m.DeclaringType).Returns(testMethodOwningType.Object);

            searcher.Setup(s => s.FindMethod(testMethodOwningType.Object, typeof(TestCleanupAttribute))).Returns(expectedCleanup.Object);

            var extractor = new MSTestExtractor(assembly.Object, searcher.Object);

            var actualCleanup = extractor.GetTestCleanup(testMethod.Object);

            Assert.AreEqual(expectedCleanup.Object, actualCleanup);
        }

        [TestMethod]
        public void ShouldGetAllTestMethodsWhenPresent()
        {
            var assembly = new Mock<IAssembly>();
            var testMethodOwningType = new Mock<IType>();
            var searcher = new Mock<IAssemblySearcher>();
            var expectedCleanup = BuildMethod("SomeClass", "Cleanup");
            var testMethod1 = BuildMethod("SomeClass", "Test1");
            var testMethod2 = BuildMethod("SomeClass", "Test2");

            searcher.Setup(s => s.FindMethods(assembly.Object, typeof(TestMethodAttribute))).Returns(new[] { testMethod1.Object, testMethod2.Object });

            var extractor = new MSTestExtractor(assembly.Object, searcher.Object);

            var actualTestMethods = extractor.GetTestMethods();

            Assert.AreEqual(testMethod1.Object, actualTestMethods[0]);
            Assert.AreEqual(testMethod2.Object, actualTestMethods[1]);
        }

        [TestMethod]
        public void AssemblySearcherShouldFindFirstMethodOfMatchingAttribute()
        {
            var searcher = new AssemblySearcher();

            var assembly = Assembly()
                           .With(Type("SomeClass")
                                 .With(Method("SomeMethod").With(typeof(TestMethodAttribute)))
                                 .With(Method("SomeOtherMethod")))
                           .With(Type("SomeOtherClass")
                                 .With(Method("SomeMethod2"))
                                 .With(Method("SomeOtherMethod2")));

            var actualMethod = searcher.FindMethod(assembly.Object, typeof(TestMethodAttribute));

            Assert.AreEqual("SomeMethod", actualMethod.Name);
        }
    }
}
