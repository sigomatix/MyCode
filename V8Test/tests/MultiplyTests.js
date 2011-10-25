function TestMultiplyShouldMultPositives()
{
    Assert.AreEqual(2, MyMath.Multiply(1, 2));
}

function TestMultiplyShouldMultNegatives()
{
    Assert.AreEqual(2, MyMath.Multiply(-1, -2));
}

function TestMultiplyShouldMultNegativesAndPositiv() {
    Assert.AreEqual(-2, MyMath.Multiply(1, -2));
}