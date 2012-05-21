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
    public class RunnerTestInitTestCleanup
    {
        [TestMethod]
        public void GivenThereIsATestClassWithATestInitThenItShouldRunTheTestInitBeforeTheTestMethodAndOnlyForTheTestMethodsInThatTestClass()
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

            runner.Run();

            methodRunner1.Verify(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { testInit1, method1, method3 }))), Times.Once());
            methodRunner2.Verify(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { testInit1, method2, method4 }))), Times.Once());
        }

        [TestMethod]
        public void GivenThereIsATestClassWithATestCleanupThenItShouldRunTheTestCleanupAfterTheTestMethodAndOnlyForTheTestMethodsInThatTestClass()
        {
            var methodRunner1 = new Mock<IMethodRunner>();
            var methodRunner2 = new Mock<IMethodRunner>();
            var assembly = new TestAssembly("");
            var type1 = assembly.AddTestClass("type1");
            var testClean1 = type1.AddTestCleanup("testClean1");
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

            runner.Run();

            methodRunner1.Verify(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { method1, testClean1, method3 }))), Times.Once());
            methodRunner2.Verify(m => m.Invoke(It.Is<IEnumerable<Method>>(e => e.SequenceEqual(new List<Method> { method2, testClean1, method4 }))), Times.Once());
        }
    }
}
