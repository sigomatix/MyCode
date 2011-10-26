TestAddditionShouldAddPositives = function()
{
    Assert.AreEqual(3, MyMath.Addition(1, 2));
}

TestAddditionShouldAddNegatives = function()
{
    Assert.AreEqual(-3, MyMath.Addition(-1, -2));
}

TestAddditionShouldAddNegativesAndPositiv = function () {
    Assert.AreEqual(-1, MyMath.Addition(1, -2));
}
