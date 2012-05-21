using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUnit.Model
{
    public abstract class MethodExecutionReport
    {
        public Method Method { get; set; }
        public string ConsoleOutput { get; set; }
    }

    public class FailedMethodExecutionReport : MethodExecutionReport
    {
        public Exception Exception { get; set; }
    }

    public class SuccessMethodExecutionReport : MethodExecutionReport
    {
    }
}
