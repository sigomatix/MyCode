using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DUnit.Model;
using DUnit.MethodRunner;

namespace DUnit.Infrastructure.MethodRunner
{
    public class InProcMethodRunner:IMethodRunner
    {
        public IEnumerable<MethodExecutionReport> Invoke(IEnumerable<Method> methods)
        {
            foreach (var meth in methods)
            {
                var assembly = System.Reflection.Assembly.LoadFrom(meth.OwningAssembly.Path);
                var type = assembly.GetType(meth.OwningType.Name);
                type.GetMethod(meth.Name).Invoke(Activator.CreateInstance(type), null);
            }

            return null;
        }
    }
}
