using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using DUnit.MethodRunner;

namespace DUnit.Model
{
    public class Runner
    {
        public List<TestAssembly> Assemblies { get; set; }
        public List<IMethodRunner> MethodRunners { get; set; }

        public Runner()
        {
            Assemblies = new List<TestAssembly>();
            MethodRunners = new List<IMethodRunner>();
        }

        public IEnumerable<MethodExecutionReport> Run()
        {
            var tasks = new List<Task>();
            var mapRunnerMethod = new Dictionary<IMethodRunner, List<Method>>();

            foreach (var runner in MethodRunners)
            {
                mapRunnerMethod.Add(runner, new List<Method>());
            }

            var enumRunner = MethodRunners.GetEnumerator();
            var assembly = Assemblies.First();
            foreach (var type in assembly.TestClasses)
            {
                foreach (var method in type.TestMethods)
                {
                    if (!enumRunner.MoveNext())
                    {
                        enumRunner = MethodRunners.GetEnumerator();
                        enumRunner.MoveNext();
                    }
                    if (assembly.AssemblyInit != null)
                    {
                        if (!mapRunnerMethod[enumRunner.Current].Contains(assembly.AssemblyInit))
                        {
                            mapRunnerMethod[enumRunner.Current].Add(assembly.AssemblyInit);
                        }
                    }
                    if (type.TestInit != null)
                    {
                        mapRunnerMethod[enumRunner.Current].Add(type.TestInit);
                    }
                    mapRunnerMethod[enumRunner.Current].Add(method);
                    if (type.TestCleanup != null)
                    {
                        mapRunnerMethod[enumRunner.Current].Add(type.TestCleanup);
                    }
                    if (assembly.AssemblyCleanup != null)
                    {
                        if (!mapRunnerMethod[enumRunner.Current].Contains(assembly.AssemblyCleanup))
                        {
                            mapRunnerMethod[enumRunner.Current].Add(assembly.AssemblyCleanup);
                        }
                    }
                }
            }

            var report = new ConcurrentStack<IEnumerable<MethodExecutionReport>>();

            foreach (var runner in mapRunnerMethod.Keys)
            {
                var r = runner; /* Enclose the correct value for the lambda */
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    report.Push(r.Invoke(mapRunnerMethod[r]));
                }));
            }

            Task.WaitAll(tasks.ToArray());

            return report.SelectMany(r => r);
        }
    }
}
