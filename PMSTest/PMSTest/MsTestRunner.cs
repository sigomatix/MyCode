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

        public MsTestRunner(IProxy proxy)
        {
            this.proxy = proxy;
        }

        public Task Run(IMethodInfo[] methodInfo)
        {
            return Task.Factory.StartNew(() => 
            {
                foreach (var method in methodInfo)
                {
                    if (method.GetCustomAttributes(typeof(TestMethodAttribute), true).Length > 0)
                    {
                        proxy.Run(method.DeclaringType.FullName, method.Name);
                    }
                }
            });
        }
    }
}
