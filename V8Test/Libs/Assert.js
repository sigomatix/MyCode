Assert =
{
    AreEqual: function (a, b) {
        if (a != b)throw (__FILENAME__ + ": Expected " + a + " but got " + b);
    }
};