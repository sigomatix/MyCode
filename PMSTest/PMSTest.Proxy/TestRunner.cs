using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PMSTest.Proxy
{
    [Serializable]
    public class TestRunner:MarshalByRefObject
    {
        public void InitAssemblies(string path)
        {
            Assembly.LoadFrom(path);
        }

        public void Run(string testClassTypeName, string testMethod)
        {
            var testClassType = Type.GetType(testClassTypeName);
            var method = testClassType.GetMethod(testMethod);

            object testClass = null;

            if (!method.IsStatic)
            {
                testClass = Activator.CreateInstance(testClassType);
            }

            method.Invoke(testClass, null);
        }

        public void RunClassInitialize(string testClassTypeName, string testMethod)
        {
            var testClassType = Type.GetType(testClassTypeName);
            var method = testClassType.GetMethod(testMethod);

            object testClass = null;

            if (!method.IsStatic)
            {
               testClass = Activator.CreateInstance(testClassType);
            }

            method.Invoke(testClass, new object[] { null });
        }

    }
}
