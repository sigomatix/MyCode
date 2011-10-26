TestDivideBySameShouldReturnOne = function()
{
    Assert.AreEqual(1, MyMath.Divide(1, 1));
}

TestDivideByZeroShouldReturnInfinity = function () {
    Assert.AreEqual(Infinity, MyMath.Divide(1, 0));
}
