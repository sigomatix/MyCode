using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DUnit.Model;

namespace DUnit.Infrastructure.AssemblyFactories
{
    public class MsTestAssemblyFactory
    {
        public TestAssembly Load(string assemblyPath)
        {
            var assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);

            var testAssembly = new TestAssembly(assemblyPath);

            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(TestClassAttribute), true).Any())
                {
                    var testClass = testAssembly.AddTestClass(type.FullName);
                    var testInit = type.GetMethods().FirstOrDefault(m => m.GetCustomAttributes(typeof(TestInitializeAttribute), true).Any());
                    var testCleanup = type.GetMethods().FirstOrDefault(m => m.GetCustomAttributes(typeof(TestCleanupAttribute), true).Any());
                    if (testInit != null)
                    {
                        testClass.AddTestInit(testInit.Name);
                    }
                    if (testCleanup != null)
                    {
                        testClass.AddTestCleanup(testCleanup.Name);
                    }
                    foreach (var method in type.GetMethods())
                    {
                        if (method.GetCustomAttributes(typeof(TestMethodAttribute), true).Any())
                        {
                            testClass.AddTestMethod(method.Name);
                        }
                        else if (method.GetCustomAttributes(typeof(AssemblyInitializeAttribute), true).Any())
                        {
                            testAssembly.SetAssemblyInit(new Method(method.Name, testClass));
                        }
                        else if (method.GetCustomAttributes(typeof(AssemblyCleanupAttribute), true).Any())
                        {
                            testAssembly.SetAssemblyCleanup(new Method(method.Name, testClass));
                        }
                    }
                }
            }
            return testAssembly;
        }
    }
}
