using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PMSTest
{
    class MSTestExtractor:ITestMethodExtractor
    {
        private IAssemblySearcher assemblySearcher;
        private IAssembly assembly;

        public MSTestExtractor(IAssembly assembly, IAssemblySearcher assemblySearcher)
        {
            this.assemblySearcher = assemblySearcher;
            this.assembly = assembly;
        }
        public IList<IMethodInfo> GetTestMethods()
        {
            return assemblySearcher.FindMethods(assembly, typeof(TestMethodAttribute));
        }

        public IMethodInfo GetAssemblyInitialise()
        {
            return assemblySearcher.FindMethod(assembly, typeof(AssemblyInitializeAttribute));
        }

        public IMethodInfo GetAssemblyCleanup()
        {
            return assemblySearcher.FindMethod(assembly, typeof(AssemblyCleanupAttribute));
        }

        public IMethodInfo GetTestInitialize(IMethodInfo methodInfo)
        {
            return assemblySearcher.FindMethod(methodInfo.DeclaringType, typeof(TestInitializeAttribute));
        }

        public IMethodInfo GetTestCleanup(IMethodInfo methodInfo)
        {
            return assemblySearcher.FindMethod(methodInfo.DeclaringType, typeof(TestCleanupAttribute));
        }
    }
}
