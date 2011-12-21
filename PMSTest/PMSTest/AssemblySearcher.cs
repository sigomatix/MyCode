using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMSTest
{
    public class AssemblySearcher:IAssemblySearcher
    {
        public IMethodInfo FindMethod(IAssembly assembly, Type attribute)
        {
            throw new NotImplementedException();
        }

        public IMethodInfo FindMethod(IType type, Type attribute)
        {
            throw new NotImplementedException();
        }

        public IList<IMethodInfo> FindMethods(IAssembly assembly, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
