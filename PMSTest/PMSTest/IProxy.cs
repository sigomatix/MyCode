using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMSTest
{
    public interface IProxy
    {
        void Run(string typeName, string methodName);
    }
}
