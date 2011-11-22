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

        [TestMethod]
        public void GivenThereAreTwotMethodsTheRunnerShouldRunnerATaskForExecutingTheseTwoTestMethod()
        {
            var proxyMock = new Mock<IProxy>();
            var runner = new MsTestRunner(proxyMock.Object);

            var dt = BuildDeclaringType("SomeTestClass");

            var testmethodMock1 = BuildMethod(dt.Object, "SomeTestMethod1", typeof(TestMethodAttribute));
            var testmethodMock2 = BuildMethod(dt.Object, "SomeTestMethod2", typeof(TestMethodAttribute));

            var methods = new IMethodInfo[] { testmethodMock1.Object, testmethodMock2.Object };

            runner.Run(methods).Wait();

            proxyMock.Verify(p => p.Run("SomeTestClass", "SomeTestMethod1"), Times.Once());
            proxyMock.Verify(p => p.Run("SomeTestClass", "SomeTestMethod2"), Times.Once());
            proxyMock.Verify(p => p.Run(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        private static Mock<IType> BuildDeclaringType(string className)
        {
            var declaringType = new Mock<IType>();
            declaringType.Setup(t => t.FullName).Returns(className);
            return declaringType;
        }

        [TestMethod]
        public void ItShouldRunTheAsemblyInitialiseOnceAndBeforeEverythingElseIfPresent()
        {
            var proxyMock = new Mock<IProxy>();
            var runner = new MsTestRunner(proxyMock.Object);

            var dt = BuildDeclaringType("SomeTestClass");
            var assemblyInit = BuildMethod(dt.Object, "AssemblyInit", typeof(AssemblyInitializeAttribute));
            var testmethodMock1 = BuildMethod(dt.Object, "SomeTestMethod1", typeof(TestMethodAttribute));
            var testmethodMock2 = BuildMethod(dt.Object, "SomeTestMethod2", typeof(TestMethodAttribute));

            var declaringType = Mock.Get(testmethodMock1.Object.DeclaringType);
            declaringType.Setup(t => t.GetMethods()).Returns(new IMethodInfo[] { assemblyInit.Object, testmethodMock1.Object, testmethodMock2.Object });
            var assemblyTypes = new IType[] { declaringType.Object };
            var assembly = new Mock<IAssembly>();
            assembly.Setup(a => a.GetTypes()).Returns(assemblyTypes);

            declaringType.Setup(t => t.Assembly).Returns(assembly.Object);

            var methods = new IMethodInfo[] { testmethodMock1.Object, testmethodMock2.Object };

            runner.Run(methods).Wait();

            proxyMock.Verify(p => p.Run("SomeTestClass", "AssemblyInit"), Times.Once());
            proxyMock.Verify(p => p.Run("SomeTestClass", "SomeTestMethod1"), Times.Once());
            proxyMock.Verify(p => p.Run("SomeTestClass", "SomeTestMethod2"), Times.Once());
            proxyMock.Verify(p => p.Run(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(3));
        }

        [TestMethod]
        public void ItShouldForTheAssemblyInitInTheWholeAssemblyAndNotJustInASpecificTestClass()
        {
            var proxyMock = new Mock<IProxy>();
            var runner = new MsTestRunner(proxyMock.Object);

            var assemblyInitDeclaringType = BuildDeclaringType("OtherClass");
            var testMethodsDeclaringType = BuildDeclaringType("SomeTestClass");
            var assemblyInit = BuildMethod(assemblyInitDeclaringType.Object, "AssemblyInit", typeof(AssemblyInitializeAttribute));
            var testmethodMock1 = BuildMethod(testMethodsDeclaringType.Object, "SomeTestMethod1", typeof(TestMethodAttribute));
            var testmethodMock2 = BuildMethod(testMethodsDeclaringType.Object, "SomeTestMethod2", typeof(TestMethodAttribute));

            assemblyInitDeclaringType.Setup(a => a.GetMethods()).Returns(new[] { assemblyInit.Object });
            testMethodsDeclaringType.Setup(a => a.GetMethods()).Returns(new[] { testmethodMock1.Object, testmethodMock2.Object });

            var assembly = new Mock<IAssembly>();
            assembly.Setup(a => a.GetTypes()).Returns(new IType[] { assemblyInitDeclaringType.Object, testMethodsDeclaringType.Object });

            testMethodsDeclaringType.Setup(t => t.Assembly).Returns(assembly.Object);
            assemblyInitDeclaringType.Setup(t => t.Assembly).Returns(assembly.Object);
            
            var methods = new IMethodInfo[] { testmethodMock1.Object, testmethodMock2.Object };

            runner.Run(methods).Wait();

            proxyMock.Verify(p => p.Run("OtherClass", "AssemblyInit"), Times.Once());
            proxyMock.Verify(p => p.Run("SomeTestClass", "SomeTestMethod1"), Times.Once());
            proxyMock.Verify(p => p.Run("SomeTestClass", "SomeTestMethod2"), Times.Once());
            proxyMock.Verify(p => p.Run(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(3));
        }

        private static Mock<IMethodInfo> BuildMethod(IType declaringType, string methodName, Type methodAttribute)
        {
            var testmethodMock = new Mock<IMethodInfo>();
            testmethodMock.Setup(m => m.GetCustomAttributes(methodAttribute, true)).Returns(new object[] { new object() });
            testmethodMock.Setup(m => m.DeclaringType).Returns(declaringType);
            testmethodMock.Setup(m => m.Name).Returns(methodName);
            return testmethodMock;
        }
    }
}
