using NUnit.Framework;
using NUnit;

namespace Task_Test_Example
{
    public class Tests
    {
        static string fileName = "testFile";

        [TestFixture]
        public class CorrectReading
        {
            [Test]
            public void SerializeAndDeserialize()
            {
                ListNode head = new ListNode(null, "head");
                ListNode tail = new ListNode(null, "tail");

                ListNode secondNode = new ListNode(null, "second");
                secondNode.Rand = secondNode;
                secondNode.ConnectToPrev(head);

                ListNode thirdNode = new ListNode(tail, "third");
                thirdNode.ConnectToPrev(secondNode);

                ListNode fourthNode = new ListNode(head, "4th");
                fourthNode.ConnectToPrev(thirdNode);

                tail.ConnectToPrev(fourthNode);

                var list = new ListRand(head, tail, 5);
                File.WriteAllText(fileName, string.Empty); //clear test file
                using (var stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    list.Serialize(stream);
                }
                using (var stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                {
                    list.Deserialize(stream);
                }
                Assert.That(list.ToString(), Is.EqualTo("(-1,'head')\n(1,'second')\n(4,'third')\n(0,'4th')\n(-1,'tail')"));
                
            }
        }
    }

}

namespace Correct_Brackets_Parentheses_etc_Parsing
{
    public class Tests
    {
        [TestFixture]
        public class CorrectReading
        {
            [TestCase("(1,'data')", 1, "data")]
            [TestCase("(1,'da'''''ta')", 1, "da'''''ta")]
            [TestCase("(1,'da)()(ta')", 1, "da)()(ta")]
            [TestCase("(1,'da')')ta')", 1, "da')')ta")]
            [TestCase(@"(1,'da\nta')", 1, "da\nta")]
            [TestCase("(1,'data1')", 1, "data1")]
            public void Brackets_Should(string input, int numberShould, string dataShould)
            {
                ListRand.FindOrderAndData(input, out int number, out string data);
                Assert.That(number, Is.EqualTo(numberShould));
                Assert.That(data, Is.EqualTo(dataShould));
            }
    }
}

}
