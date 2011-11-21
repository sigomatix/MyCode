using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMSTest
{
    public class TestRunnerStub:ITestRunner
    {
        public IList<string> ExecutedMethods { get; private set; }

        public object Run(IMethodInfo[] methodInfo)
        {
            ExecutedMethods = methodInfo.Select(m => m.Name).ToList();
            return null;
        }
    }
}
