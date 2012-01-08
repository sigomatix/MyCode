using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PMSTest
{
    public class MsTestRunner : ITestRunner
    {
        private IProxy proxy;
        private ITestMethodExtractor testMethodExtractor;

        public MsTestRunner(IProxy proxy, ITestMethodExtractor testMethodExtractor)
        {
            this.proxy = proxy;
            this.testMethodExtractor = testMethodExtractor;
        }

        public Task Run(IMethodInfo[] methodInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                var assemblyInit = testMethodExtractor.GetAssemblyInitialise();
                var assemblyCleanup = testMethodExtractor.GetAssemblyCleanup();

                if (assemblyInit != null)
                {
                    proxy.Run(assemblyInit.DeclaringType.FullName, assemblyInit.Name);
                }

                foreach (var method in methodInfo)
                {
                    var testInitialize = testMethodExtractor.GetTestInitialize(method);
                    var testCleanup = testMethodExtractor.GetTestCleanup(method);

                    if (testInitialize != null)
                    {
                        proxy.Run(testInitialize.DeclaringType.FullName, testInitialize.Name);
                    }

                    proxy.Run(method.DeclaringType.FullName, method.Name);

                    if (testCleanup != null)
                    {
                        proxy.Run(testCleanup.DeclaringType.FullName, testCleanup.Name);
                    }
                }

                if (assemblyCleanup != null)
                {
                    proxy.Run(assemblyCleanup.DeclaringType.FullName, assemblyCleanup.Name);
                }
            });
        }
    }
}
