function TestDivideBySameShouldReturnOne()
{
    Assert.AreEqual(1, MyMath.Divide(1, 1));
}

function TestDivideByZeroShouldReturnInfinity() {
    Assert.AreEqual(Infinity, MyMath.Divide(1, 0));
}
