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
                IMethodInfo assemblyInit = testMethodExtractor.GetAssemblyInitialise();
                IMethodInfo assemblyCleanup = testMethodExtractor.GetAssemblyCleanup();

                if (assemblyInit != null)
                {
                    proxy.Run(assemblyInit.DeclaringType.FullName, assemblyInit.Name);
                }

                foreach (var method in methodInfo)
                {
                    proxy.Run(method.DeclaringType.FullName, method.Name);
                }

                if (assemblyCleanup != null)
                {
                    proxy.Run(assemblyCleanup.DeclaringType.FullName, assemblyCleanup.Name);
                }
            });
        }
    }
}
