using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace V8Test
{
    public class TestFile
    {
        public string FilePath { get; set; }
        public List<string> TestMethods { get; set; }

        public TestFile(string path)
        {
            FilePath = path;
        }
    }


    public class JSTestGenerator
    {
        private List<TestFile> testFiles = new List<TestFile>();
        private List<string> includeFiles = new List<string>();
        private string relocationPath;
        private static object locker = new Object();

        public JSTestGenerator()
        {
        }

        public void SetTestFilesRelocationPath(string path)
        {
            relocationPath = path;
        }

        public void AddTestFile(string p)
        {
            var testFile = new TestFile(p);
            testFiles.Add(testFile);

            var context = new IronJS.Hosting.CSharp.Context();
            {
                context.Execute(File.ReadAllText(p));
                var methodsList = (string)context.Execute(
@"b ='';
                for(var m in this)
                {
                    if(m.toString().toLowerCase().indexOf('test')>=0)
                    {
                        b+=m+';'
                    }
                };
                b;");
                testFile.TestMethods = methodsList.Trim(';').Split(';').ToList();
            }

        }

        public IList<string> GetAllTestMethods()
        {
            return testFiles.SelectMany(file => file.TestMethods).ToList();
        }

        public string GenerateTestCodeForMethod(string p)
        {
            var builder = new StringBuilder();
            builder.Append("var context = new IronJS.Hosting.CSharp.Context();\r\n{\r\n");
            foreach (var include in includeFiles) builder.Append(string.Format("    context.Execute(File.ReadAllText(@\"{0}\"));\r\n", include));
            var test = testFiles.FirstOrDefault(t => t.TestMethods.Contains(p));
            string finalPath = test.FilePath;
            if (!string.IsNullOrEmpty(relocationPath))
            {
                finalPath = Path.Combine(relocationPath, Path.GetFileName(test.FilePath));
            }
            builder.Append(string.Format("    context.Execute(@\"__FILENAME__ = '{0}';\".Replace(@\"\\\", @\"\\\\\"));\r\n", finalPath));
            builder.Append(string.Format("    context.Execute(File.ReadAllText(@\"{0}\"));\r\n", finalPath));

            builder.Append(string.Format("    context.Execute(\"{0}();\");\r\n", p));
            builder.Append("}");
            return builder.ToString();
        }

        public void AddIncludeFile(string p)
        {
            includeFiles.Add(p);
        }
    }
}
