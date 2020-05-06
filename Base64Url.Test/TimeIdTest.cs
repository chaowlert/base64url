using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Base64Url.Test
{
    [TestClass]
    public class TimeIdTest
    {
        [TestMethod]
        public void TestSortableId()
        {
            var date1 = new DateTime(2000, 1, 1, 0, 0, 0);
            var date2 = date1.AddSeconds(1);
            var a = TimeId.NewSortableId(date1);
            var b = TimeId.NewSortableId(date2);

            Assert.IsTrue(a.CompareTo(b) > 0);
            Assert.AreEqual(date2, TimeId.ToDateTime(b));

            var c = TimeId.NewSortableId(date1, true);
            var d = TimeId.NewSortableId(date2, true);

            Assert.IsTrue(c.CompareTo(d) < 0);
        }

        [TestMethod]
        public void TestTimeId()
        {
            var date1 = new DateTime(2000, 1, 1, 0, 0, 0);
            var a = TimeId.GetTimeId(date1);
            Assert.AreEqual(date1, TimeId.ToDateTime(a));
        }

        [TestMethod]
        public void TestNewSortableId()
        {
            var a = TimeId.NewSortableId();
            var b = TimeId.NewSortableId();

            Assert.AreNotEqual(a, b);
        }
    }
}
