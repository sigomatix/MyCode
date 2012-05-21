using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using DUnit.Model;
using DUnit.MethodRunner;

namespace DUnit.Model.Tests
{
    /*
     * Ecriture premier test u pour mettre en place un model de base qui va permettre de tester l'algo principal
     * Le premier test vérifie que l'on peut lancer l'execution d'un seul test
     * AJout d'un second test pour vérifier que l'on peut lancer 2 tests
     * 
     * Question: Comment continuer
     * 1. Soit on continue de dev l'algo principal => Par exemple un nouveau test avec 2 tests et 2 ressources d'execution
     * 2. Soit commencer à implémenter/tester les dépendance (Factory du runner\Resource execution)
     * 
     * Le choix 1. permet de rester dans le même contexte et continuer à déveloper le model, l'avantage est de mieux définir les contraintes du systeme, l'inconvénient est que l'on obtient un programme qui fonctionne plus tardivement
     * Le choix 2. permet d'otenir une application qui fonctionne plus rapidement avec un cas simple au risque de devoir implémenter sans connaitre toutes les contraintes
     * 
     * 
     * On découvre une friction dans le dev
     * Ecrire le moteur générique avec TDD se prête bien et est fun
     * Ecrire les implem d'infrastructure avec TDD devient vite une punition (Assembly factory et method factory)
     * Car on doit mocker bcp de chose pour au final tester de la logique qui est peut interessante....
     * Si écrire les tests pour les factories/repositories n'est pas interessant pourquoi ne pas les couvrir uniquement via des tests d'intégration pur seulement ?
     * 
     * Car au final le role des repositories/factories est de gérer de l'intégration avec l'infra, donc des tests d'intégration sont plus appropriés
     * Alors que le moteur/model représente de la logique pur, du TDD/test unitaire pur devrait donc s'appliquer uniquement
     *
     * On applique donc SRP au niveau de la façon de tester: 
     * Test d'intégration, test uniquement les objets qui ont des responsabilités d'intégration avec l'infrastructure
     * Test unitaire, test uniquement les objets qui ont de la logique pure, étant découplé de tout soucis d'infrastructure
     * 
     * On va donc prendre l'approche suivante:
     * Découpler au maximum la logique pur dans le model et utiliser TDD
     * Le reste sera donc des dépendances  d'infrastructures et sera testé uniquement via des tests d'intégrations
     * On ne vas donc pas faire d'effort à ce niveau à rendre le code d'infrastructure découplé et testable.
     * C'est à dire que si l'infrastructure possède des dépendances on va dire que nous ne sommes pas intéressés à rendre ces dépendances pluggables via des interfaces
     * Si l'infra est une dépendance pour le model on a donc un découplage via des interfaces
     * En revanche, les dépendances de l'infra sont pour le model des dépendances de dépendances (dépandances de second niveau), il est donc moins important d'apporter de l'évolutivilité à ce niveau
     * 
     * VRAIMENT, le truc est de pouvoir faire du TDD en restant dans le "FUN" et la créativité.
     * faire du TDD sur du code d'infra apporte peu de valeur car:
     * - N'est pas fun car on ne test souvent pas de logique métier
     * - Pénible car on se retrouve à introduire bcp d'interface juste pour tester de logique très simple
     * - Compléxifie inutilement l'architecture
     * - Allonge inutilement le temps au bout du quel on va avoir un résultat montrable
     * - Crée une suite de test à faible valeur ajoutée: la vraie valeur c'est le métier, pas les dépendances d'infra qui peuvent changer avec le temps (changement de framework par exemple, il faudra jeter tous les tests unitaire)
     * - Temps mal investit. Les tests Unitaires d'infra prennent un temps qui peut être mieux investit sur du test unitaire métier.
     */

    [TestClass]
    public class RunnerTest
    {
        [TestMethod]
        public void GivenThereIsOnlyOneTestMethodThenTheRunnerShouldRunThatMethod()
        {
            var methodRunner = new Mock<IMethodRunner>();
            var method = new TestAssembly("").AddTestClass("").AddTestMethod("method1");

            var runner = new Runner()
            {
                MethodRunners = new List<IMethodRunner>
                {
                    methodRunner.Object
                },
                Assemblies = new List<TestAssembly>()
                {
                    method.OwningTestAssembly 
                },
            };
            var expectedReport = new List<MethodExecutionReport> 
            { 
                new SuccessMethodExecutionReport(){Method = method,ConsoleOutput="Executed"}
            };
            methodRunner.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.First() == method))).Returns(expectedReport);

            var actualReport = runner.Run();
            Assert.AreEqual(expectedReport[0], actualReport.First());
        }

        [TestMethod]
        public void GivenThereIsTwoTestMethodsInTheSameTestClassAndOneRunnerThenItShouldRunTheseMethods()
        {
            var methodRunner = new Mock<IMethodRunner>();
            var type = new TestAssembly("").AddTestClass("");
            TestMethod method1 = type.AddTestMethod("method1");
            TestMethod method2 = type.AddTestMethod("method2");

            var runner = new Runner()
            {
                MethodRunners = new List<IMethodRunner>
                {
                    methodRunner.Object
                },
                Assemblies = new List<TestAssembly>()
                {
                    type.TestAssembly
                },
            };
            runner.Run();
            methodRunner.Verify(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { method1, method2 }))), Times.Once());

            var report1 = new SuccessMethodExecutionReport() { Method = method1, ConsoleOutput = "Executed 1" };
            var report2 = new SuccessMethodExecutionReport() { Method = method2, ConsoleOutput = "Executed 2" };
            var expectedReport = new List<MethodExecutionReport> 
            { 
                report1,
                report2
            };
            methodRunner.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { method1, method2 })))).Returns(expectedReport);

            var actualReport = runner.Run();
            Assert.AreEqual(actualReport.Count(), 2);
            Assert.IsTrue(actualReport.Contains(report1));
            Assert.IsTrue(actualReport.Contains(report2));
        }

        [TestMethod]
        public void GivenThereIsTwoTestMethodsInTheSameTestClassAndTwoRunnersThenEachRunnerShouldRunOneOfTheMethods()
        {
            var methodRunner1 = new Mock<IMethodRunner>();
            var methodRunner2 = new Mock<IMethodRunner>();
            var type = new TestAssembly("").AddTestClass("");
            TestMethod method1 = type.AddTestMethod("method1");
            TestMethod method2 = type.AddTestMethod("method2");

            var runner = new Runner()
            {
                MethodRunners = new List<IMethodRunner>
                {
                    methodRunner1.Object, methodRunner2.Object
                },
                Assemblies = new List<TestAssembly>()
                {
                    type.TestAssembly
                },
            };

            var report1 = new SuccessMethodExecutionReport() { Method = method1, ConsoleOutput = "Executed 1" };
            var report2 = new SuccessMethodExecutionReport() { Method = method2, ConsoleOutput = "Executed 2" };
            var reportForRunner1 = new List<MethodExecutionReport> { report1 };
            var reportForRunner2 = new List<MethodExecutionReport> { report2 };
            methodRunner1.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { method1 })))).Returns(reportForRunner1);
            methodRunner2.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { method2 })))).Returns(reportForRunner2);

            var actualReport = runner.Run();
            Assert.AreEqual(actualReport.Count(), 2);
            Assert.IsTrue(actualReport.Contains(report1));
            Assert.IsTrue(actualReport.Contains(report2));
        }

        [TestMethod]
        public void GivenThereIsTwoTestClassesWithOneTestMethodInEachAndTwoRunnersThenEachRunnerShouldRunOneOfTheMethods()
        {
            var methodRunner1 = new Mock<IMethodRunner>();
            var methodRunner2 = new Mock<IMethodRunner>();
            var assembly = new TestAssembly("");
            var method1 = assembly.AddTestClass("type1").AddTestMethod("method1");
            var method2 = assembly.AddTestClass("type2").AddTestMethod("method2");

            var runner = new Runner()
            {
                MethodRunners = new List<IMethodRunner>
                {
                    methodRunner1.Object, methodRunner2.Object
                },
                Assemblies = new List<TestAssembly>()
                {
                    assembly
                },
            };

            var report1 = new SuccessMethodExecutionReport() { Method = method1, ConsoleOutput = "Executed 1" };
            var report2 = new SuccessMethodExecutionReport() { Method = method2, ConsoleOutput = "Executed 2" };
            var reportForRunner1 = new List<MethodExecutionReport> { report1 };
            var reportForRunner2 = new List<MethodExecutionReport> { report2 };
            methodRunner1.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { method1 })))).Returns(reportForRunner1);
            methodRunner2.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { method2 })))).Returns(reportForRunner2);

            var actualReport = runner.Run();
            Assert.AreEqual(actualReport.Count(), 2);
            Assert.IsTrue(actualReport.Contains(report1));
            Assert.IsTrue(actualReport.Contains(report2));
        }

        [TestMethod]
        public void GivenThereIsTwoTestClassesWithTwoTestMethodsInEachAndTwoRunnersThenEachRunnerShouldRunTwoOfTheseMethods()
        {
            var methodRunner1 = new Mock<IMethodRunner>();
            var methodRunner2 = new Mock<IMethodRunner>();
            var assembly = new TestAssembly("");
            var type1 = assembly.AddTestClass("type1");
            var method1 = type1.AddTestMethod("method1");
            var method2 = type1.AddTestMethod("method2");
            var type2 = assembly.AddTestClass("type2");
            var method3 = type2.AddTestMethod("method3");
            var method4 = type2.AddTestMethod("method4");

            var runner = new Runner()
            {
                MethodRunners = new List<IMethodRunner>
                {
                    methodRunner1.Object, methodRunner2.Object
                },
                Assemblies = new List<TestAssembly>()
                {
                    assembly
                },
            };

            var report1 = new SuccessMethodExecutionReport() { Method = method1, ConsoleOutput = "Executed 1" };
            var report2 = new SuccessMethodExecutionReport() { Method = method2, ConsoleOutput = "Executed 2" };
            var report3 = new SuccessMethodExecutionReport() { Method = method3, ConsoleOutput = "Executed 3" };
            var report4 = new SuccessMethodExecutionReport() { Method = method4, ConsoleOutput = "Executed 4" };
            var reportForRunner1 = new List<MethodExecutionReport> { report1, report3 };
            var reportForRunner2 = new List<MethodExecutionReport> { report2, report4 };
            methodRunner1.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { method1, method3 })))).Returns(reportForRunner1);
            methodRunner2.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { method2, method4 })))).Returns(reportForRunner2);

            var actualReport = runner.Run();
            Assert.AreEqual(actualReport.Count(), 4);
            Assert.IsTrue(actualReport.Contains(report1));
            Assert.IsTrue(actualReport.Contains(report2));
            Assert.IsTrue(actualReport.Contains(report3));
            Assert.IsTrue(actualReport.Contains(report4));
        }

        [TestMethod]
        public void GivenThereIsATestClassWithATestInitThenItShouldRunTheTestInitOnlyForTheTestMethodsInThatTestClass()
        {
            var methodRunner1 = new Mock<IMethodRunner>();
            var methodRunner2 = new Mock<IMethodRunner>();
            var assembly = new TestAssembly("");
            var type1 = assembly.AddTestClass("type1");
            var testInit1 = type1.AddTestInit("testInit1");
            var method1 = type1.AddTestMethod("method1");
            var method2 = type1.AddTestMethod("method2");
            var type2 = assembly.AddTestClass("type2");
            var method3 = type2.AddTestMethod("method3");
            var method4 = type2.AddTestMethod("method4");

            var runner = new Runner()
            {
                MethodRunners = new List<IMethodRunner>
                {
                    methodRunner1.Object, methodRunner2.Object
                },
                Assemblies = new List<TestAssembly>()
                {
                    assembly
                },
            };

            var report1 = new SuccessMethodExecutionReport() { Method = method1, ConsoleOutput = "Executed 1" };
            var report2 = new SuccessMethodExecutionReport() { Method = method2, ConsoleOutput = "Executed 2" };
            var report3 = new SuccessMethodExecutionReport() { Method = method3, ConsoleOutput = "Executed 3" };
            var report4 = new SuccessMethodExecutionReport() { Method = method4, ConsoleOutput = "Executed 4" };
            var reportForRunner1 = new List<MethodExecutionReport> { report1, report3 };
            var reportForRunner2 = new List<MethodExecutionReport> { report2, report4 };
            methodRunner1.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { testInit1, method1, method3 })))).Returns(reportForRunner1);
            methodRunner2.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { testInit1, method2, method4 })))).Returns(reportForRunner2);

            runner.Run();

            var actualReport = runner.Run();
            Assert.AreEqual(actualReport.Count(), 4);
            Assert.IsTrue(actualReport.Contains(report1));
            Assert.IsTrue(actualReport.Contains(report2));
            Assert.IsTrue(actualReport.Contains(report3));
            Assert.IsTrue(actualReport.Contains(report4));
        }

        [TestMethod]
        public void GivenThereIsATestClassWithAAssemblyInitThenItShouldRunTheAssemblyInitOnceForAllTheTestsInTheAssembly()
        {
            var methodRunner1 = new Mock<IMethodRunner>();
            var methodRunner2 = new Mock<IMethodRunner>();
            var assembly = new TestAssembly("");
            var type1 = assembly.AddTestClass("type1");
            var assemblyInit = assembly.SetAssemblyInit(new Method("assemblyInit",type1));
            var method1 = type1.AddTestMethod("method1");
            var method2 = type1.AddTestMethod("method2");
            var type2 = assembly.AddTestClass("type2");
            var method3 = type2.AddTestMethod("method3");
            var method4 = type2.AddTestMethod("method4");

            var runner = new Runner()
            {
                MethodRunners = new List<IMethodRunner>
                {
                    methodRunner1.Object, methodRunner2.Object
                },
                Assemblies = new List<TestAssembly>()
                {
                    assembly
                },
            };

            var report1 = new SuccessMethodExecutionReport() { Method = method1, ConsoleOutput = "Executed 1" };
            var report2 = new SuccessMethodExecutionReport() { Method = method2, ConsoleOutput = "Executed 2" };
            var report3 = new SuccessMethodExecutionReport() { Method = method3, ConsoleOutput = "Executed 3" };
            var report4 = new SuccessMethodExecutionReport() { Method = method4, ConsoleOutput = "Executed 4" };
            var reportForRunner1 = new List<MethodExecutionReport> { report1, report3 };
            var reportForRunner2 = new List<MethodExecutionReport> { report2, report4 };
            methodRunner1.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { assemblyInit, method1, method3 })))).Returns(reportForRunner1);
            methodRunner2.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { assemblyInit, method2, method4 })))).Returns(reportForRunner2);

            runner.Run();

            var actualReport = runner.Run();
            Assert.AreEqual(actualReport.Count(), 4);
            Assert.IsTrue(actualReport.Contains(report1));
            Assert.IsTrue(actualReport.Contains(report2));
            Assert.IsTrue(actualReport.Contains(report3));
            Assert.IsTrue(actualReport.Contains(report4));
        }

        [TestMethod]
        public void GivenThereIsOnlyOneFalingTestMethodThenTheRunnerShouldReportThatTheMethodFailed()
        {
            var methodRunner = new Mock<IMethodRunner>();
            var method = new TestAssembly("").AddTestClass("").AddTestMethod("method1");

            var runner = new Runner()
            {
                MethodRunners = new List<IMethodRunner>
                {
                    methodRunner.Object
                },
                Assemblies = new List<TestAssembly>()
                {
                    method.OwningTestAssembly 
                },
            };
            var expectedReport = new List<MethodExecutionReport> 
            { 
                new FailedMethodExecutionReport(){Method = method,ConsoleOutput="Executed"}
            };
            methodRunner.Setup(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.First() == method))).Returns(expectedReport);

            var actualReport = runner.Run();
            Assert.AreEqual(expectedReport[0], actualReport.First());
        }
    }
}
