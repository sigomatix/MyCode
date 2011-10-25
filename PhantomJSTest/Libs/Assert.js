Assert =
{
    AreEqual: function (a, b) {
        if (a == b) {
            console.log("Expected " + a + " but got " + b);
            phantom.exit(1);
        }
    }
};