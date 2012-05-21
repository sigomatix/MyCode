using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUnit.Model
{
    public class Method
    {
        public Assembly OwningAssembly 
        {
            get
            {
                return OwningType.OwningAssembly;
            }
        }

        public Type OwningType { get; private set; }
        public string Name { get; private set; }

        public Method(string name, Type owningType)
        {
            Name = name;
            OwningType = owningType;
        }
    }
}
