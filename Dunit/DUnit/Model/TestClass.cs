using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUnit.Model
{
    public class TestClass:Type
    {
        public TestMethod TestInit { get; private set; }
        public TestMethod TestCleanup { get; private set; }
        public IList<TestMethod> TestMethods { get; private set; }
        
        public TestClass(string name, TestAssembly assembly):base(name, assembly)
        {
            TestMethods = new List<TestMethod>();
            this.TestAssembly = assembly;
        }

        public TestMethod AddTestMethod(string Name)
        {
            var m = new TestMethod(Name, this);
            TestMethods.Add(m);
            return m;
        }

        public TestAssembly TestAssembly { get; private set; }

        public TestMethod AddTestInit(string name)
        {
            TestInit = new TestMethod(name, this);
            return TestInit;
        }

        public TestMethod AddTestCleanup(string name)
        {
            TestCleanup = new TestMethod(name, this);
            return TestCleanup;
        }
    }
}
