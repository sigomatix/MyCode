using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Proxy
{
    public class Worker : MarshalByRefObject
    {
        private Dictionary<Type, object> cache = new Dictionary<Type, object>();

        public string Invoke(string assemblyPath, string typeName, string methodName)
        {
            var sw = new StringWriter();
            Console.SetOut(sw);
            var assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);
            var type = assembly.GetType(typeName);

            object instance;
            if (!cache.TryGetValue(type, out instance))
            {
                instance = Activator.CreateInstance(type);
                cache.Add(type, instance);
            }
            type.GetMethod(methodName).Invoke(instance, null);
            return sw.ToString();
        }
    }
}
