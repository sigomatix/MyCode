using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMSTest
{
    public class StubLog
    {
        public string Type { get; set; }
        public string Method { get; set; }
    }

    public class ProxyStub:IProxy
    {
        public List<StubLog> Log { get; private set; }

        public ProxyStub()
        {
            Log = new List<StubLog>();
        }

        public void Run(string typeName, string methodName)
        {
            Log.Add(new StubLog() {Type = typeName, Method = methodName });
        }
    }
}
