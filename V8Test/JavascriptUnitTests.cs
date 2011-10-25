

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class JavascriptUnitTests
    {

		[TestMethod]
		public void TestAddditionShouldAddPositives()
		{
			using(var context = new Noesis.Javascript.JavascriptContext())
			{
			    context.Run(File.ReadAllText(@"scripts\Math.js"));
			    context.Run(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Run(File.ReadAllText(@"Libs\Assert.js"));
			    context.Run(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Run(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Run(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Run(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Run("TestAddditionShouldAddPositives();");
			}
		}

		[TestMethod]
		public void TestAddditionShouldAddNegatives()
		{
			using(var context = new Noesis.Javascript.JavascriptContext())
			{
			    context.Run(File.ReadAllText(@"scripts\Math.js"));
			    context.Run(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Run(File.ReadAllText(@"Libs\Assert.js"));
			    context.Run(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Run(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Run(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Run(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Run("TestAddditionShouldAddNegatives();");
			}
		}

		[TestMethod]
		public void TestAddditionShouldAddNegativesAndPositiv()
		{
			using(var context = new Noesis.Javascript.JavascriptContext())
			{
			    context.Run(File.ReadAllText(@"scripts\Math.js"));
			    context.Run(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Run(File.ReadAllText(@"Libs\Assert.js"));
			    context.Run(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Run(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Run(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Run(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Run("TestAddditionShouldAddNegativesAndPositiv();");
			}
		}

		[TestMethod]
		public void TestViewNewsArticleShouldOpenASmallDialogForPriceAlerts()
		{
			using(var context = new Noesis.Javascript.JavascriptContext())
			{
			    context.Run(File.ReadAllText(@"scripts\Math.js"));
			    context.Run(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Run(File.ReadAllText(@"Libs\Assert.js"));
			    context.Run(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Run(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Run(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Run(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Run("TestViewNewsArticleShouldOpenASmallDialogForPriceAlerts();");
			}
		}

		[TestMethod]
		public void TestViewNewsArticleShouldOpenABigDialogForRegularNews()
		{
			using(var context = new Noesis.Javascript.JavascriptContext())
			{
			    context.Run(File.ReadAllText(@"scripts\Math.js"));
			    context.Run(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Run(File.ReadAllText(@"Libs\Assert.js"));
			    context.Run(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Run(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Run(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Run(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Run("TestViewNewsArticleShouldOpenABigDialogForRegularNews();");
			}
		}

		[TestMethod]
		public void TestViewNewsArticleShouldPutCorrectStylesWhenOpeningAndClosingTheLightBox()
		{
			using(var context = new Noesis.Javascript.JavascriptContext())
			{
			    context.Run(File.ReadAllText(@"scripts\Math.js"));
			    context.Run(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Run(File.ReadAllText(@"Libs\Assert.js"));
			    context.Run(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Run(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Run(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Run(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Run("TestViewNewsArticleShouldPutCorrectStylesWhenOpeningAndClosingTheLightBox();");
			}
		}

		[TestMethod]
		public void TestDivideBySameShouldReturnOne()
		{
			using(var context = new Noesis.Javascript.JavascriptContext())
			{
			    context.Run(File.ReadAllText(@"scripts\Math.js"));
			    context.Run(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Run(File.ReadAllText(@"Libs\Assert.js"));
			    context.Run(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Run(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Run(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Run(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Run("TestDivideBySameShouldReturnOne();");
			}
		}

		[TestMethod]
		public void TestDivideByZeroShouldReturnInfinity()
		{
			using(var context = new Noesis.Javascript.JavascriptContext())
			{
			    context.Run(File.ReadAllText(@"scripts\Math.js"));
			    context.Run(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Run(File.ReadAllText(@"Libs\Assert.js"));
			    context.Run(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Run(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Run(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Run(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Run("TestDivideByZeroShouldReturnInfinity();");
			}
		}

		[TestMethod]
		public void TestMultiplyShouldMultPositives()
		{
			using(var context = new Noesis.Javascript.JavascriptContext())
			{
			    context.Run(File.ReadAllText(@"scripts\Math.js"));
			    context.Run(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Run(File.ReadAllText(@"Libs\Assert.js"));
			    context.Run(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Run(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Run(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Run(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Run("TestMultiplyShouldMultPositives();");
			}
		}

		[TestMethod]
		public void TestMultiplyShouldMultNegatives()
		{
			using(var context = new Noesis.Javascript.JavascriptContext())
			{
			    context.Run(File.ReadAllText(@"scripts\Math.js"));
			    context.Run(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Run(File.ReadAllText(@"Libs\Assert.js"));
			    context.Run(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Run(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Run(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Run(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Run("TestMultiplyShouldMultNegatives();");
			}
		}

		[TestMethod]
		public void TestMultiplyShouldMultNegativesAndPositiv()
		{
			using(var context = new Noesis.Javascript.JavascriptContext())
			{
			    context.Run(File.ReadAllText(@"scripts\Math.js"));
			    context.Run(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Run(File.ReadAllText(@"Libs\Assert.js"));
			    context.Run(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Run(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Run(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Run(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Run("TestMultiplyShouldMultNegativesAndPositiv();");
			}
		}

     
    }
}


