using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMSTest
{
    public class AssemblySearcher:IAssemblySearcher
    {
        /* TODO: Maybe this should throw if more than one ? */
        public IMethodInfo FindMethod(IAssembly assembly, Type attribute)
        {
            return FindMethods(assembly,attribute).FirstOrDefault();
        }

        public IMethodInfo FindMethod(IType type, Type attribute)
        {
            var q = from m in type.GetMethods()
                    where m.GetCustomAttributes(attribute, true).Any()
                    select m;
            return q.FirstOrDefault();
        }

        public IList<IMethodInfo> FindMethods(IAssembly assembly, Type attribute)
        {
            var q = from t in assembly.GetTypes()
                    from m in t.GetMethods()
                    /* Pass hard coded true as a parameter, let the caller decide */
                    where m.GetCustomAttributes(attribute, true).Any()
                    select m;
            return q.ToList();
        }
    }
}
