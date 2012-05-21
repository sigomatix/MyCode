using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.IO.Pipes;
using System.Diagnostics;
using System.Threading.Tasks;
using DUnit.Model;
using DUnit.Infrastructure.AssemblyFactories;
using DUnit.Infrastructure.MethodRunner;

namespace DUnit.Model.Tests
{
    /// <summary>
    /// Description résumée pour IntegrationTests
    /// </summary>
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void RunOneTest()
        {
            var runner = new Runner();
            runner.Assemblies.Add(new MsTestAssemblyFactory().Load("OneTestMethodOnly.dll"));
            runner.MethodRunners.Add(new AppDomainMethodRunner());

            var report = runner.Run();

            Assert.AreEqual(1, report.Count());
            Assert.AreEqual("TestMethod1", report.First().Method.Name);
            Assert.IsTrue(report.First() is SuccessMethodExecutionReport);
        }

        [TestMethod]
        public void RunTwoTestMethodInTheSameTestClassOnTwoRunners()
        {
            var runner = new Runner();
            runner.Assemblies.Add(new MsTestAssemblyFactory().Load("TwoTestMethodsInSameTestClass.dll"));
            runner.MethodRunners.Add(new AppDomainMethodRunner());
            runner.MethodRunners.Add(new AppDomainMethodRunner());

            var watch = Stopwatch.StartNew();
            var report = runner.Run();
            watch.Stop();
            Assert.IsTrue(report.Select(r => r.Method.Name).Contains("TestMethod1"));
            Assert.IsTrue(report.Select(r => r.Method.Name).Contains("TestMethod2"));
            Assert.AreEqual(2, report.Count());
            Assert.IsTrue(watch.Elapsed < TimeSpan.FromSeconds(6));
        }

        [TestMethod]
        public void RunTwoTestClassesWithOneTestMethodInEachOnTwoRunners()
        {
            var runner = new Runner();
            runner.Assemblies.Add(new MsTestAssemblyFactory().Load("TwoTestClassesWithOneTestMethodInEach.dll"));
            runner.MethodRunners.Add(new AppDomainMethodRunner());
            runner.MethodRunners.Add(new AppDomainMethodRunner());

            var watch = Stopwatch.StartNew();
            var report = runner.Run();
            watch.Stop();
            Assert.IsTrue(report.Select(r => r.Method.Name).Contains("TestMethod1"));
            Assert.IsTrue(report.Select(r => r.Method.Name).Contains("TestMethod2"));
            Assert.AreEqual(2, report.Count());
            Assert.IsTrue(watch.Elapsed < TimeSpan.FromSeconds(6));
        }

        [TestMethod]
        public void RunTestAssemblyWithTestInitAndTestCleanup()
        {
            var runner = new Runner();
            runner.Assemblies.Add(new MsTestAssemblyFactory().Load("TestInitTestCleanup.dll"));
            runner.MethodRunners.Add(new AppDomainMethodRunner());
            var report = runner.Run();
        }

        [TestMethod]
        public void RunTestAssemblyWithAssemblyInitAndAssemblyCleanup()
        {
            var runner = new Runner();
            runner.Assemblies.Add(new MsTestAssemblyFactory().Load("AssemblyInitAssemblyCleanup.dll"));
            runner.MethodRunners.Add(new AppDomainMethodRunner());
            var report = runner.Run();
        }

        [TestMethod]
        public void OneFailingTestMethod()
        {
            var runner = new Runner();
            runner.Assemblies.Add(new MsTestAssemblyFactory().Load("OneFailingTestMethod.dll"));
            runner.MethodRunners.Add(new AppDomainMethodRunner());
            var report = runner.Run().First() as FailedMethodExecutionReport;

            Assert.AreEqual("TestMethod1", report.Method.Name);
            Assert.IsInstanceOfType(report.Exception, typeof(AssertFailedException));
        }
    }
}
