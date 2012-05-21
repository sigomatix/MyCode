using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUnit.Model
{
    public class TestMethod : Method
    {
        public TestAssembly OwningTestAssembly
        {
            get
            {
                return OwningTestClass.TestAssembly;
            }
        }
        public TestClass OwningTestClass { get; private set; }

        public TestMethod(string name, TestClass testClass)
            : base(name, testClass)
        {
            OwningTestClass = testClass;
        }
    }
}
