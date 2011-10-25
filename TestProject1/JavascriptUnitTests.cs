

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
		public void TestAddditionShouldAddNegatives()
		{
			var ctx = new IronJS.Hosting.CSharp.Context();
			Action<string> failAction = error => Assert.Fail(error + " in " + "AdditionTests.js");
			var failFunc = IronJS.Native.Utils.CreateFunction(ctx.Environment, 1, failAction);
			ctx.SetGlobal("FAIL", failFunc);
			ctx.ExecuteFile("Assert.js");
			ctx.ExecuteFile("Math.js");
			ctx.ExecuteFile("Dashboard.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\AdditionTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DashboardTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DivideTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\MultiplyTests.js");
			ctx.Execute("TestAddditionShouldAddNegatives();");
		}

		[TestMethod]
		public void TestAddditionShouldAddNegativesAndPositiv()
		{
			var ctx = new IronJS.Hosting.CSharp.Context();
			Action<string> failAction = error => Assert.Fail(error + " in " + "AdditionTests.js");
			var failFunc = IronJS.Native.Utils.CreateFunction(ctx.Environment, 1, failAction);
			ctx.SetGlobal("FAIL", failFunc);
			ctx.ExecuteFile("Assert.js");
			ctx.ExecuteFile("Math.js");
			ctx.ExecuteFile("Dashboard.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\AdditionTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DashboardTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DivideTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\MultiplyTests.js");
			ctx.Execute("TestAddditionShouldAddNegativesAndPositiv();");
		}

		[TestMethod]
		public void TestAddditionShouldAddPositives()
		{
			var ctx = new IronJS.Hosting.CSharp.Context();
			Action<string> failAction = error => Assert.Fail(error + " in " + "AdditionTests.js");
			var failFunc = IronJS.Native.Utils.CreateFunction(ctx.Environment, 1, failAction);
			ctx.SetGlobal("FAIL", failFunc);
			ctx.ExecuteFile("Assert.js");
			ctx.ExecuteFile("Math.js");
			ctx.ExecuteFile("Dashboard.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\AdditionTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DashboardTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DivideTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\MultiplyTests.js");
			ctx.Execute("TestAddditionShouldAddPositives();");
		}

		[TestMethod]
		public void TestViewNewsArticleShouldOpenABigDialogForRegularNews()
		{
			var ctx = new IronJS.Hosting.CSharp.Context();
			Action<string> failAction = error => Assert.Fail(error + " in " + "AdditionTests.js");
			var failFunc = IronJS.Native.Utils.CreateFunction(ctx.Environment, 1, failAction);
			ctx.SetGlobal("FAIL", failFunc);
			ctx.ExecuteFile("Assert.js");
			ctx.ExecuteFile("Math.js");
			ctx.ExecuteFile("Dashboard.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\AdditionTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DashboardTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DivideTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\MultiplyTests.js");
			ctx.Execute("TestViewNewsArticleShouldOpenABigDialogForRegularNews();");
		}

		[TestMethod]
		public void TestViewNewsArticleShouldOpenASmallDialogForPriceAlerts()
		{
			var ctx = new IronJS.Hosting.CSharp.Context();
			Action<string> failAction = error => Assert.Fail(error + " in " + "AdditionTests.js");
			var failFunc = IronJS.Native.Utils.CreateFunction(ctx.Environment, 1, failAction);
			ctx.SetGlobal("FAIL", failFunc);
			ctx.ExecuteFile("Assert.js");
			ctx.ExecuteFile("Math.js");
			ctx.ExecuteFile("Dashboard.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\AdditionTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DashboardTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DivideTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\MultiplyTests.js");
			ctx.Execute("TestViewNewsArticleShouldOpenASmallDialogForPriceAlerts();");
		}

		[TestMethod]
		public void TestViewNewsArticleShouldPutCorrectStylesWhenOpeningAndClosingTheLightBox()
		{
			var ctx = new IronJS.Hosting.CSharp.Context();
			Action<string> failAction = error => Assert.Fail(error + " in " + "AdditionTests.js");
			var failFunc = IronJS.Native.Utils.CreateFunction(ctx.Environment, 1, failAction);
			ctx.SetGlobal("FAIL", failFunc);
			ctx.ExecuteFile("Assert.js");
			ctx.ExecuteFile("Math.js");
			ctx.ExecuteFile("Dashboard.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\AdditionTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DashboardTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DivideTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\MultiplyTests.js");
			ctx.Execute("TestViewNewsArticleShouldPutCorrectStylesWhenOpeningAndClosingTheLightBox();");
		}

		[TestMethod]
		public void TestDivideBySameShouldReturnOne()
		{
			var ctx = new IronJS.Hosting.CSharp.Context();
			Action<string> failAction = error => Assert.Fail(error + " in " + "AdditionTests.js");
			var failFunc = IronJS.Native.Utils.CreateFunction(ctx.Environment, 1, failAction);
			ctx.SetGlobal("FAIL", failFunc);
			ctx.ExecuteFile("Assert.js");
			ctx.ExecuteFile("Math.js");
			ctx.ExecuteFile("Dashboard.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\AdditionTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DashboardTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DivideTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\MultiplyTests.js");
			ctx.Execute("TestDivideBySameShouldReturnOne();");
		}

		[TestMethod]
		public void TestDivideByZeroShouldReturnInfinity()
		{
			var ctx = new IronJS.Hosting.CSharp.Context();
			Action<string> failAction = error => Assert.Fail(error + " in " + "AdditionTests.js");
			var failFunc = IronJS.Native.Utils.CreateFunction(ctx.Environment, 1, failAction);
			ctx.SetGlobal("FAIL", failFunc);
			ctx.ExecuteFile("Assert.js");
			ctx.ExecuteFile("Math.js");
			ctx.ExecuteFile("Dashboard.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\AdditionTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DashboardTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DivideTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\MultiplyTests.js");
			ctx.Execute("TestDivideByZeroShouldReturnInfinity();");
		}

		[TestMethod]
		public void TestMultiplyShouldMultNegatives()
		{
			var ctx = new IronJS.Hosting.CSharp.Context();
			Action<string> failAction = error => Assert.Fail(error + " in " + "AdditionTests.js");
			var failFunc = IronJS.Native.Utils.CreateFunction(ctx.Environment, 1, failAction);
			ctx.SetGlobal("FAIL", failFunc);
			ctx.ExecuteFile("Assert.js");
			ctx.ExecuteFile("Math.js");
			ctx.ExecuteFile("Dashboard.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\AdditionTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DashboardTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DivideTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\MultiplyTests.js");
			ctx.Execute("TestMultiplyShouldMultNegatives();");
		}

		[TestMethod]
		public void TestMultiplyShouldMultNegativesAndPositiv()
		{
			var ctx = new IronJS.Hosting.CSharp.Context();
			Action<string> failAction = error => Assert.Fail(error + " in " + "AdditionTests.js");
			var failFunc = IronJS.Native.Utils.CreateFunction(ctx.Environment, 1, failAction);
			ctx.SetGlobal("FAIL", failFunc);
			ctx.ExecuteFile("Assert.js");
			ctx.ExecuteFile("Math.js");
			ctx.ExecuteFile("Dashboard.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\AdditionTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DashboardTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DivideTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\MultiplyTests.js");
			ctx.Execute("TestMultiplyShouldMultNegativesAndPositiv();");
		}

		[TestMethod]
		public void TestMultiplyShouldMultPositives()
		{
			var ctx = new IronJS.Hosting.CSharp.Context();
			Action<string> failAction = error => Assert.Fail(error + " in " + "AdditionTests.js");
			var failFunc = IronJS.Native.Utils.CreateFunction(ctx.Environment, 1, failAction);
			ctx.SetGlobal("FAIL", failFunc);
			ctx.ExecuteFile("Assert.js");
			ctx.ExecuteFile("Math.js");
			ctx.ExecuteFile("Dashboard.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\AdditionTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DashboardTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\DivideTests.js");
			ctx.ExecuteFile(@"C:\Users\sigomatix\Documents\My Dropbox\Dropbox\vsprojects\IronJS-NativeObjectDemo\TestProject1\tests\MultiplyTests.js");
			ctx.Execute("TestMultiplyShouldMultPositives();");
		}

     
    }
}


