using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMSTest
{
    public interface IAssemblySearcher
    {
        IMethodInfo FindMethod(IAssembly assembly, Type attribute);
        IMethodInfo FindMethod(IType type, Type attribute);
        IList<IMethodInfo> FindMethods(IAssembly assembly, Type type);
    }
}
