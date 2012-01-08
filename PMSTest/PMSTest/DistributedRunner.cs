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
        private IEnumerable<ITestRunner> testRunners;
        private ITestMethodExtractor testMethodExtractor;

        public DistributedRunner(IEnumerable<ITestRunner> testRunners, ITestMethodExtractor testMethodExtractor)
        {
            this.testRunners = testRunners;
            this.testMethodExtractor = testMethodExtractor;
        }

        public Task Run()
        {
            var allTestMethods = testMethodExtractor.GetTestMethods();

            var testMethodsDistribution = new Dictionary<ITestRunner, IList<IMethodInfo>>();
            foreach (var runner in testRunners)
            {
                testMethodsDistribution.Add(runner, new List<IMethodInfo>());
            }

            var testMethodsDistributionEnum = testMethodsDistribution.GetEnumerator();
            foreach (var testMethod in allTestMethods)
            {
                KeyValuePair<ITestRunner, IList<IMethodInfo>> distribution;

                if (testMethodsDistributionEnum.MoveNext())
                {
                    distribution = testMethodsDistributionEnum.Current;
                }
                else
                {
                    testMethodsDistributionEnum = testMethodsDistribution.GetEnumerator();
                    testMethodsDistributionEnum.MoveNext();
                    distribution = testMethodsDistributionEnum.Current;
                }

                distribution.Value.Add(testMethod);
            }

            var allRunnersTasks = testMethodsDistribution.Select(r=>r.Key.Run(r.Value.ToArray()));
            
            return Task.Factory.StartNew( () => Task.WaitAll(allRunnersTasks.ToArray()));
        }
    }
}
