using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUnit.Model
{
    public class TestAssembly:Assembly
    {
        public IList<TestClass> TestClasses { get; private set; }
        

        public TestAssembly(string path):base(path)
        {
            TestClasses = new List<TestClass>();
        }

        public TestClass AddTestClass(string name)
        {
            var t = new TestClass(name, this);
            TestClasses.Add(t);
            return t;
        }

        public Method SetAssemblyInit(Method method)
        {
            AssemblyInit = method;
            return AssemblyInit;
        }

        public Method SetAssemblyCleanup(Method method)
        {
            AssemblyCleanup = method;
            return AssemblyCleanup;
        }

        public Method AssemblyInit { get; private set; }
        public Method AssemblyCleanup { get; private set; }
    }
}
