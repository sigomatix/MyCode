using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noesis.Javascript;
using System.IO;

namespace V8Test
{

    public class JSTestGenerator
    {
        private List<string> testFiles = new List<string>();
        private List<string> includeFiles = new List<string>();
        private string relocationPath;

        public JSTestGenerator()
        {
        }

        public void SetTestFilesRelocationPath(string path)
        {
            relocationPath = path;
        }

        public void AddTestFile(string p)
        {
            testFiles.Add(p);
        }

        public IList<string> GetAllTestMethods()
        {
            using (var context = new JavascriptContext())
            {
                foreach (var testFile in testFiles)
                {
                    context.Run(File.ReadAllText(testFile));
                }

                var methodsList = (string)context.Run(
@"var b ='';
for(var m in this)
{
    if(m.toString().toLowerCase().indexOf('test')>=0)
    {
        b+=m+';'
    }
};
b;");
                return methodsList.Trim(';').Split(';').ToList();
            }
        }

        public string GenerateTestCodeForMethod(string p)
        {
            var builder = new StringBuilder();
            builder.Append("using(var context = new Noesis.Javascript.JavascriptContext())\r\n{\r\n");
            foreach (var include in includeFiles) builder.Append(string.Format("    context.Run(File.ReadAllText(@\"{0}\"));\r\n", include));
            foreach (var test in testFiles)
            {
                string finalPath = test;
                if (!string.IsNullOrEmpty(relocationPath))
                {
                    finalPath = Path.Combine(relocationPath, Path.GetFileName(test));
                }
                builder.Append(string.Format("    context.Run(File.ReadAllText(@\"{0}\"));\r\n", finalPath));
            }
            builder.Append(string.Format("    context.Run(\"{0}();\");\r\n", p));
            builder.Append("}");
            return builder.ToString();
        }

        public void AddIncludeFile(string p)
        {
            includeFiles.Add(p);
        }
    }
}
