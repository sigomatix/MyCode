using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSTest
{
    public class TestRunnerStub:ITestRunner
    {
        public IList<string> ExecutedMethods { get; private set; }

        public Task Run(IMethodInfo[] methodInfo)
        {
            return Task.Factory.StartNew( () => ExecutedMethods = methodInfo.Select(m => m.Name).ToList());
        }
    }
}
