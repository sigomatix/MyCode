using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Proxy;
using DUnit.Model;
using System.IO;
using DUnit.MethodRunner;

namespace DUnit.Infrastructure.MethodRunner
{
    public class AppDomainMethodRunner : IMethodRunner
    {
        private AppDomain ad;

        public AppDomainMethodRunner()
        {
            var setup = new AppDomainSetup()
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                //ConfigurationFile = Assembly.GetExecutingAssembly().CodeBase+".config"
            };

            ad = AppDomain.CreateDomain("New domain", null, setup);

        }

        public IEnumerable<MethodExecutionReport> Invoke(IEnumerable<Method> methods)
        {
            var report = new List<MethodExecutionReport>();
            Worker remoteWorker = (Worker)ad.CreateInstanceAndUnwrap(
                "Proxy",
                "Proxy.Worker");
            foreach (var meth in methods)
            {
                try
                {
                    var console = remoteWorker.Invoke(meth.OwningAssembly.Path, meth.OwningType.Name, meth.Name);
                    report.Add(new SuccessMethodExecutionReport()
                    {
                        Method = meth,
                        ConsoleOutput = console
                    });
                }
                catch (Exception ex)
                {
                    report.Add(new FailedMethodExecutionReport()
                        {
                            Method = meth,
                            Exception = ex.InnerException
                        });
                }
            }

            return report;
        }
    }
}
