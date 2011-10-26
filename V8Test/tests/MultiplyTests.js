TestMultiplyShouldMultPositives = function()
{
    Assert.AreEqual(2, MyMath.Multiply(1, 2));
}

TestMultiplyShouldMultNegatives = function()
{
    Assert.AreEqual(2, MyMath.Multiply(-1, -2));
}

TestMultiplyShouldMultNegativesAndPositiv = function () {
    Assert.AreEqual(-2, MyMath.Multiply(1, -2));
}