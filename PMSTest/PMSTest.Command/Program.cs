using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMSTest.Proxy;
using System.IO;

namespace PMSTest.Command
{
    class Program
    {
        static void Main(string[] args)
        {
            var testAssemblyPath = @"C:\Dashboard2010\Dev\source\ICISDashboard.Specs\bin\Local FT\ICISDashboard.Specs.dll";
            var testAssembly = Assembly.LoadFrom(@testAssemblyPath);

            var testMethods = (from t in testAssembly.GetTypes()
                               from m in t.GetMethods()
                               where t.GetCustomAttributes(typeof(TestClassAttribute), false).Length > 0
                               && m.GetCustomAttributes(typeof(TestMethodAttribute), false).Length == 1
                               let testInitialize = t.GetMethods().FirstOrDefault(ti => ti.GetCustomAttributes(typeof(TestInitializeAttribute), false).Length == 1)
                               let classInitialize = t.GetMethods().FirstOrDefault(ti => ti.GetCustomAttributes(typeof(ClassInitializeAttribute), false).Length == 1)
                               select new { TestMethod = m, TestInitialize = testInitialize, ClassInitialize = classInitialize });

            var assemblyInitialize = (from t in testAssembly.GetTypes()
                                     from m in t.GetMethods()
                                     where m.IsStatic && m.GetCustomAttributes(typeof(AssemblyInitializeAttribute), false).Length == 1
                                     select m).FirstOrDefault();


            // Construct and initialize settings for a second AppDomain.
            AppDomainSetup ads = new AppDomainSetup();
            ads.ApplicationBase = Path.GetDirectoryName(testAssemblyPath);
            /*ads.PrivateBinPath = Path.GetDirectoryName(testAssemblyPath);*/
            ads.DisallowBindingRedirects = false;
            ads.DisallowCodeDownload = true;
            ads.ConfigurationFile = testAssemblyPath + ".config";

            // Create the second AppDomain.
            AppDomain ad2 = AppDomain.CreateDomain("AD #2", null, ads);

            var testRunner = (TestRunner)ad2.CreateInstanceFromAndUnwrap("PMSTest.proxy.dll", "PMSTest.Proxy.TestRunner");
            testRunner.InitAssemblies(testAssemblyPath);

            foreach (var testMethod in testMethods)
            {
                // ad2.DoCallBack(new CrossAppDomainDelegate(testMethod.DoCall));
                testRunner.RunClassInitialize(assemblyInitialize.DeclaringType.AssemblyQualifiedName, assemblyInitialize.Name);
                testRunner.RunClassInitialize(testMethod.ClassInitialize.DeclaringType.AssemblyQualifiedName, testMethod.ClassInitialize.Name);
                testRunner.Run(testMethod.TestInitialize.DeclaringType.AssemblyQualifiedName, testMethod.TestInitialize.Name);
                testRunner.Run(testMethod.TestMethod.DeclaringType.AssemblyQualifiedName, testMethod.TestMethod.Name);
            }
        }
    }
}
