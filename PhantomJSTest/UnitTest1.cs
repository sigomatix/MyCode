using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace PhantomJSTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GenerateJS()
        {
            var testPath = @"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\PhantomJSTest\tests\AdditionTests.js";
            var rewrittenTestForRun = Path.Combine(Path.GetDirectoryName(testPath), Path.GetFileName(testPath) + ".generated");
            var rewrittenTestForReflection = Path.Combine(Path.GetDirectoryName(testPath), Path.GetFileName(testPath) + ".reflection");
            var testContent = File.ReadAllText(testPath);

            var phantomJsPath = @"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\PhantomJSTest\phantomjs.exe";

            // run phantom to get the list of all tests
            var allMethods = GetAllTestsMethodsInScript(testContent, phantomJsPath, rewrittenTestForReflection);

            for (int i = 0; i < 200; i++)
            {
                var builder = new StringBuilder(testContent);
                builder.Insert(0, string.Format("phantom.injectJs(\"{0}\");" + Environment.NewLine, @"C:\\Users\\sigomatix\\Documents\\My Dropbox\\Dropbox\\vsprojects\\IronJS-NativeObjectDemo\\PhantomJSTest\\libs\\Assert.js"));
                builder.Insert(0, string.Format("phantom.injectJs(\"{0}\");" + Environment.NewLine, @"C:\\Users\\sigomatix\\Documents\\My Dropbox\\Dropbox\\vsprojects\\IronJS-NativeObjectDemo\\IronJSDemo\\scripts\\Math.js"));
                builder.AppendLine(allMethods[0] + "();");
                builder.AppendLine("phantom.exit();");
                File.WriteAllText(rewrittenTestForRun, builder.ToString());

                var p = new Process();
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.LoadUserProfile = false;
                p.StartInfo.FileName = phantomJsPath;
                p.StartInfo.Arguments = "\"" + rewrittenTestForRun + "\"";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.UseShellExecute = false;
                p.Start();

                var errors = p.StandardError.ReadToEnd();
                var output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                if (p.ExitCode > 0)
                {
                    //throw new AssertFailedException(output);
                }
            }
        }

        private static IList<string> GetAllTestsMethodsInScript(string testContent, string phantomJsPath, string rewrittenTestForReflection)
        {

            var builder = new StringBuilder(testContent);
            builder.AppendLine("for(var i in this)");
            builder.AppendLine("{");
            builder.AppendLine("if(typeof(this[i])=='function')console.log(i);");
            builder.AppendLine("}");
            builder.AppendLine("phantom.exit();");
            File.WriteAllText(rewrittenTestForReflection, builder.ToString());

            var p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.LoadUserProfile = false;
            p.StartInfo.FileName = phantomJsPath;
            p.StartInfo.Arguments = "\"" + rewrittenTestForReflection + "\"";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();

            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return (from testName in output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                    where testName.StartsWith("test", StringComparison.InvariantCultureIgnoreCase)
                    select testName).ToList();
        }
    }
}
