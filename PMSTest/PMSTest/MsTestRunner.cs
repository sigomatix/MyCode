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
                var assemblyInit = (from t in methodInfo.First().DeclaringType.Assembly.GetTypes()
                                   from m in t.GetMethods()
                                   where m.GetCustomAttributes(typeof(AssemblyInitializeAttribute), true).Length > 0
                                   select m).FirstOrDefault();

                if (assemblyInit != null)
                {
                    proxy.Run(assemblyInit.DeclaringType.FullName, assemblyInit.Name);
                }

                foreach (var method in methodInfo)
                {
                    proxy.Run(method.DeclaringType.FullName, method.Name);
                }
            });
        }
    }
}
