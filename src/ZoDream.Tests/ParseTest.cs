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
            var str = @"fn test  //����ű���
click(99,1)  //�������
300         //�ȴ�300����
drag(1,22,88,1)   //��ק(1,22)�Ķ�����(88,1)
 //���б�ʾ�ν���
if (0,0,20,20)=        //�ж������Ƿ�Ϊ���أ����Լ�Ϊ�ж϶Խ��ߵ�����
test()    //ִ�нű���
else
exit   //�˳�";
            var parse = new Tokenizer();
            var items = parse.Parse(str);
            Assert.AreEqual(items.Count, 10);
        }
    }
}