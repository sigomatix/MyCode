using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMSTest
{
    public interface IMethodInfo
    {
        object[] GetCustomAttributes(Type type, bool p);
        string Name { get; }
    }
}
