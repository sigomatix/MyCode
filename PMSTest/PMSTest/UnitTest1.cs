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

namespace PMSTest
{




    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void WhenDistributedOn4RunnersItShouldRunOnceTheAssemblyInitializeOnEachRunner()
        //{
        //    var assemblyResolver = new Mock<IAssemblyResolver>();
        //    var testAssembly = new Mock<Assembly>();
        //    var testClassType = new Mock<Type>();
        //    var assemblyInitialize = new Mock<MethodInfo>();

        //    assemblyResolver.Setup(a => a.LoadFrom(@"c:\someAssembly.dll")).Returns(testAssembly.Object);
        //    testAssembly.Setup(t => t.GetTypes()).Returns(new Type[] { testClassType.Object });
        //    testClassType.Setup(t => t.GetMethods()).Returns(new MethodInfo[] { assemblyInitialize.Object });
        //    assemblyInitialize.Setup(a => a.IsStatic).Returns(true);
        //    assemblyInitialize.Setup(a => a.GetCustomAttributes(typeof(AssemblyInitializeAttribute), false)).Returns(new Object[] { new Object() });
        //    assemblyInitialize.Verify(a=>a.Invoke(It.IsAny<object>(), It.IsAny<object[]>()),Times.Exactly(4));

        //    var runner = new DistributedRunner(4, assemblyResolver.Object);

        //    var runTask = runner.Run(@"c:\someAssembly.dll");
        //    runTask.Wait();

        //}

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
    }
}
