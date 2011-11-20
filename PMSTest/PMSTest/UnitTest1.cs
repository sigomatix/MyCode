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
                        m.Setup(a => a.GetCustomAttributes(typeof(TestMethodAttribute), false)).Returns(new Object[] { new Object() });
                    }
                    return m;
                });
            var testRunners = Enumerable.Range(1,3).Select(i=>  new Mock<ITestRunner>() );

            assemblyResolver.Setup(a => a.LoadFrom(@"c:\someAssembly.dll")).Returns(testAssembly.Object);
            testAssembly.Setup(t => t.GetTypes()).Returns(new IType[] { testClassType.Object });
            testClassType.Setup(t => t.GetMethods()).Returns(methods.Select(m=>m.Object).ToArray());

            var runner = new DistributedRunner(testRunners.Select(t => t.Object), assemblyResolver.Object);

            var runTask = runner.Run(@"c:\someAssembly.dll");
            runTask.Wait();

            var runnersList = testRunners.ToList();
            var methodList = methods.ToList();
            runnersList[0].Verify(r => r.Run(It.Is<IMethodInfo[]>(p => p[0] == methodList[0].Object && p[1] == methodList[3].Object && p[2] == methodList[6].Object && p.Length == 3)), Times.Exactly(1));
            runnersList[1].Verify(r => r.Run(It.Is<IMethodInfo[]>(p => p[0] == methodList[1].Object && p[1] == methodList[4].Object && p[2] == methodList[7].Object && p.Length == 3)), Times.Exactly(1));
            runnersList[2].Verify(r => r.Run(It.Is<IMethodInfo[]>(p => p[0] == methodList[2].Object && p[1] == methodList[5].Object && p.Length == 2)), Times.Exactly(1));

        }
    }
}
