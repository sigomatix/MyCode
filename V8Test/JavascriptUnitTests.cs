

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
			var context = new IronJS.Hosting.CSharp.Context();
			{
			    context.Execute(File.ReadAllText(@"scripts\Math.js"));
			    context.Execute(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Execute(File.ReadAllText(@"Libs\Assert.js"));
			    context.Execute(@"__FILENAME__ = 'tests\AdditionTests.js';".Replace(@"\", @"\\"));
			    context.Execute(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Execute("TestAddditionShouldAddPositives();");
			}
		}

		[TestMethod]
		public void TestAddditionShouldAddNegatives()
		{
			var context = new IronJS.Hosting.CSharp.Context();
			{
			    context.Execute(File.ReadAllText(@"scripts\Math.js"));
			    context.Execute(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Execute(File.ReadAllText(@"Libs\Assert.js"));
			    context.Execute(@"__FILENAME__ = 'tests\AdditionTests.js';".Replace(@"\", @"\\"));
			    context.Execute(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Execute("TestAddditionShouldAddNegatives();");
			}
		}

		[TestMethod]
		public void TestAddditionShouldAddNegativesAndPositiv()
		{
			var context = new IronJS.Hosting.CSharp.Context();
			{
			    context.Execute(File.ReadAllText(@"scripts\Math.js"));
			    context.Execute(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Execute(File.ReadAllText(@"Libs\Assert.js"));
			    context.Execute(@"__FILENAME__ = 'tests\AdditionTests.js';".Replace(@"\", @"\\"));
			    context.Execute(File.ReadAllText(@"tests\AdditionTests.js"));
			    context.Execute("TestAddditionShouldAddNegativesAndPositiv();");
			}
		}

		[TestMethod]
		public void TestViewNewsArticleShouldOpenASmallDialogForPriceAlerts()
		{
			var context = new IronJS.Hosting.CSharp.Context();
			{
			    context.Execute(File.ReadAllText(@"scripts\Math.js"));
			    context.Execute(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Execute(File.ReadAllText(@"Libs\Assert.js"));
			    context.Execute(@"__FILENAME__ = 'tests\DashboardTests.js';".Replace(@"\", @"\\"));
			    context.Execute(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Execute("TestViewNewsArticleShouldOpenASmallDialogForPriceAlerts();");
			}
		}

		[TestMethod]
		public void TestViewNewsArticleShouldOpenABigDialogForRegularNews()
		{
			var context = new IronJS.Hosting.CSharp.Context();
			{
			    context.Execute(File.ReadAllText(@"scripts\Math.js"));
			    context.Execute(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Execute(File.ReadAllText(@"Libs\Assert.js"));
			    context.Execute(@"__FILENAME__ = 'tests\DashboardTests.js';".Replace(@"\", @"\\"));
			    context.Execute(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Execute("TestViewNewsArticleShouldOpenABigDialogForRegularNews();");
			}
		}

		[TestMethod]
		public void TestViewNewsArticleShouldPutCorrectStylesWhenOpeningAndClosingTheLightBox()
		{
			var context = new IronJS.Hosting.CSharp.Context();
			{
			    context.Execute(File.ReadAllText(@"scripts\Math.js"));
			    context.Execute(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Execute(File.ReadAllText(@"Libs\Assert.js"));
			    context.Execute(@"__FILENAME__ = 'tests\DashboardTests.js';".Replace(@"\", @"\\"));
			    context.Execute(File.ReadAllText(@"tests\DashboardTests.js"));
			    context.Execute("TestViewNewsArticleShouldPutCorrectStylesWhenOpeningAndClosingTheLightBox();");
			}
		}

		[TestMethod]
		public void TestDivideBySameShouldReturnOne()
		{
			var context = new IronJS.Hosting.CSharp.Context();
			{
			    context.Execute(File.ReadAllText(@"scripts\Math.js"));
			    context.Execute(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Execute(File.ReadAllText(@"Libs\Assert.js"));
			    context.Execute(@"__FILENAME__ = 'tests\DivideTests.js';".Replace(@"\", @"\\"));
			    context.Execute(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Execute("TestDivideBySameShouldReturnOne();");
			}
		}

		[TestMethod]
		public void TestDivideByZeroShouldReturnInfinity()
		{
			var context = new IronJS.Hosting.CSharp.Context();
			{
			    context.Execute(File.ReadAllText(@"scripts\Math.js"));
			    context.Execute(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Execute(File.ReadAllText(@"Libs\Assert.js"));
			    context.Execute(@"__FILENAME__ = 'tests\DivideTests.js';".Replace(@"\", @"\\"));
			    context.Execute(File.ReadAllText(@"tests\DivideTests.js"));
			    context.Execute("TestDivideByZeroShouldReturnInfinity();");
			}
		}

		[TestMethod]
		public void TestMultiplyShouldMultPositives()
		{
			var context = new IronJS.Hosting.CSharp.Context();
			{
			    context.Execute(File.ReadAllText(@"scripts\Math.js"));
			    context.Execute(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Execute(File.ReadAllText(@"Libs\Assert.js"));
			    context.Execute(@"__FILENAME__ = 'tests\MultiplyTests.js';".Replace(@"\", @"\\"));
			    context.Execute(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Execute("TestMultiplyShouldMultPositives();");
			}
		}

		[TestMethod]
		public void TestMultiplyShouldMultNegatives()
		{
			var context = new IronJS.Hosting.CSharp.Context();
			{
			    context.Execute(File.ReadAllText(@"scripts\Math.js"));
			    context.Execute(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Execute(File.ReadAllText(@"Libs\Assert.js"));
			    context.Execute(@"__FILENAME__ = 'tests\MultiplyTests.js';".Replace(@"\", @"\\"));
			    context.Execute(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Execute("TestMultiplyShouldMultNegatives();");
			}
		}

		[TestMethod]
		public void TestMultiplyShouldMultNegativesAndPositiv()
		{
			var context = new IronJS.Hosting.CSharp.Context();
			{
			    context.Execute(File.ReadAllText(@"scripts\Math.js"));
			    context.Execute(File.ReadAllText(@"scripts\Dashboard.js"));
			    context.Execute(File.ReadAllText(@"Libs\Assert.js"));
			    context.Execute(@"__FILENAME__ = 'tests\MultiplyTests.js';".Replace(@"\", @"\\"));
			    context.Execute(File.ReadAllText(@"tests\MultiplyTests.js"));
			    context.Execute("TestMultiplyShouldMultNegativesAndPositiv();");
			}
		}

     
    }
}


