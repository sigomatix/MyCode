using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DUnit.Model;

namespace DUnit.MethodRunner
{
    public interface IMethodRunner
    {
        IEnumerable<MethodExecutionReport> Invoke(IEnumerable<Method> methods);
    }
}
