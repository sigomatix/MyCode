function TestAddditionShouldAddPositives()
{
    Assert.AreEqual(3, MyMath.Addition(1, 2));
}

function TestAddditionShouldAddNegatives()
{
    Assert.AreEqual(-3, MyMath.Addition(-1, -2));
}

function TestAddditionShouldAddNegativesAndPositiv() {
    Assert.AreEqual(-1, MyMath.Addition(1, -2));
}
