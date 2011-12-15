using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMSTest
{
    public interface ITestMethodExtractor
    {
        IList<IMethodInfo> GetTestMethods();

        IMethodInfo GetAssemblyInitialise();
        IMethodInfo GetAssemblyCleanup();
    }
}
