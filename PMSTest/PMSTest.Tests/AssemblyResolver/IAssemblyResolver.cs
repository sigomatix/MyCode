using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace PMSTest
{
    public interface IAssemblyResolver
    {
        IAssembly LoadFrom(string p);
    }
}
