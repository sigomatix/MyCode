using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUnit.Model
{
    public class Type
    {
        public string Name { get; private set; }
        public Type(string Name, Assembly owningAssembly)
        {
            this.Name = Name;
            this.OwningAssembly = owningAssembly;
        }

        public Assembly OwningAssembly { get; private set; }
    }
}
