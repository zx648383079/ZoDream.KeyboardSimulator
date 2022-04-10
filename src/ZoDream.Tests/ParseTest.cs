using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;
using ZoDream.Shared.Input;
using ZoDream.Shared.OS.WinApi;
using ZoDream.Shared.Parser;
using ZoDream.Shared.Utils;

namespace ZoDream.Tests
{
    [TestClass]
    public class ParseTest
    {
        [TestMethod]
        public void TestParse()
        {
            var key = Key.W;
            Assert.AreEqual(InputNativeMethods.MapVirtualKey((uint)key,
                (uint)MappingType.VK_TO_VSC), 0x11u);
        }

        [TestMethod]
        public void TestInt()
        {
            var k = "OemPeriod";
            Assert.AreEqual((Key)Enum.Parse(typeof(Key), k), Key.OemPeriod);
        }


    }
}