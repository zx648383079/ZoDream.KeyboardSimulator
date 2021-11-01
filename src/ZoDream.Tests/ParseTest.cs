using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZoDream.Shared.Parser;

namespace ZoDream.Tests
{
    [TestClass]
    public class ParseTest
    {
        [TestMethod]
        public void TestParse()
        {
            var str = @"fn test  //定义脚本段
click(99,1)  //点击坐标
300         //等待300毫秒
drag(1,22,88,1)   //拖拽(1,22)的东西到(88,1)
 //空行表示段结束
if (0,0,20,20)=        //判断区域是否为像素，可以简化为判断对角线的像素
test()    //执行脚本段
else
exit   //退出";
            var parse = new Tokenizer();
            var items = parse.Parse(str);
            Assert.AreEqual(items.Count, 10);
        }
    }
}