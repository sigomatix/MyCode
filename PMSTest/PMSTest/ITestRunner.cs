using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMSTest
{
    public interface ITestRunner
    {
        object Run(IMethodInfo[] methodInfo);
    }
}
