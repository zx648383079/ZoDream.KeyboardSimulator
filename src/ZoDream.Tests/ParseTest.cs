using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;
using ZoDream.Shared.Input;
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
            var str = @"fn test // 定义方法
            DoubleClick(Right) //双击鼠标右键
            2000  // 延时2s
            Click() //点击左键
             // 方法结束
            1000 //延迟1s
            Move(20,20) // 鼠标移动到点(20,20)
            Click()   //点击左键
            if 0,20,40,60=md5  // 判断点(0,20)到(40,60)的直线路径上的颜色值是否为，请通过拾取按钮自动框选获取
                test()        // 为True则执行 定义的方法test
            endif";
            var parse = new Lexer();
            parse.Open(str);
            for (int i = 0; i < 100; i++)
            {
                parse.NextToken();
            }
            Assert.AreEqual(parse.NextToken(), Token.Fn);
        }

        [TestMethod]
        public void TestInt()
        {
            var k = "OemPeriod";
            Assert.AreEqual((Key)Enum.Parse(typeof(Key), k), Key.OemPeriod);
        }


    }
}