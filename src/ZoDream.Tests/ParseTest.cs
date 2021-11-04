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
            var str = @"fn test // ���巽��
            DoubleClick(Right) //˫������Ҽ�
            2000  // ��ʱ2s
            Click() //������
             // ��������
            1000 //�ӳ�1s
            Move(20,20) // ����ƶ�����(20,20)
            Click()   //������
            if 0,20,40,60=md5  // �жϵ�(0,20)��(40,60)��ֱ��·���ϵ���ɫֵ�Ƿ�Ϊ����ͨ��ʰȡ��ť�Զ���ѡ��ȡ
                test()        // ΪTrue��ִ�� ����ķ���test
            endif";
            var parse = new Tokenizer();
            var items = parse.Parse(str);
            Assert.AreEqual(items.Count, 10);
        }


    }
}