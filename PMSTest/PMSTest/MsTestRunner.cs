using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PMSTest
{
    public class MsTestRunner : ITestRunner
    {
        private IProxy proxy;

        public MsTestRunner(IProxy proxy)
        {
            this.proxy = proxy;
        }

        public Task Run(IMethodInfo[] methodInfo)
        {
            return Task.Factory.StartNew(() => { proxy.Run("SomeTestClass", "SomeTestMethod"); });
        }
    }
}
