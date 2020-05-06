using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Base64Url.Test
{
    [TestClass]
    public class Base64Test
    {
        [TestMethod]
        public void TestConvertFromString()
        {
            var base64 = Base64.GetBase64("foo/bar");
            Assert.AreEqual("foo/bar", Base64.ToString(base64));
        }
    }
}
