using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PMSTest
{
    class DistributedRunner
    {
        private IAssemblyResolver assemblyResolver;
        private IEnumerable<ITestRunner> testRunners;

        public DistributedRunner(IEnumerable<ITestRunner> testRunners, IAssemblyResolver assemblyResolver)
        {
            this.testRunners = testRunners;
            this.assemblyResolver = assemblyResolver;
        }

        public Task Run(string testAssemblyPath)
        {
            var assembly = assemblyResolver.LoadFrom(testAssemblyPath);

            var allTestMethods = from t in assembly.GetTypes()
                                 from m in t.GetMethods()
                                 where m.GetCustomAttributes(typeof (TestMethodAttribute), true).Length == 1
                                 select m;

            var testMethodsDistribution = new Dictionary<ITestRunner, IList<IMethodInfo>>();
            foreach(var runner in testRunners)
            {
                testMethodsDistribution.Add(runner, new List<IMethodInfo>());
            }

            var testMethodsDistributionEnum = testMethodsDistribution.GetEnumerator();
            foreach(var testMethod in allTestMethods)
            {
                IList<IMethodInfo> distribution;

                if (testMethodsDistributionEnum.MoveNext())
                {
                    distribution = testMethodsDistributionEnum.Current;
                }
                else
                {
                    testMethodsDistributionEnum.Reset();
                    testMethodsDistributionEnum.MoveNext();
                    distribution = testMethodsDistributionEnum.Current;
                }

                distribution.Add(testMethod);
            }

            var task = new Task( () =>
                                     {
                                         foreach()
                                     });
            task.Start();
            return task;
        }
    }
}
