using NUnit.Framework;
using NUnit;

namespace TestProject
{
    public class Tests
    {

        [TestFixture]
        public class CorrectReading
        {
            //[Test]
            //public void Serialization_1()
            //{
            //    string input = @"[-1,0][1,1][4,2][0,3][-1,4]";
            //    Assertion(input);
            //}

            [Test]
            //public void Serialization_2()///сделать чрез testcase+case sob[]aka
            //{
            //    string input = @"[-1,a][-1,b][-1,c][-1,d]";
            //    Assertion(input);
            //}

            //public void Assertion(string input)
            //{
            //    var list = input.ConvertToListRand();
            //    FileStream stream = new FileStream("file.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //    stream.Position = 0;
            //    var numberedDictionary = list.GetOrderedDictionary();
            //    var output = list.ConvertToString(numberedDictionary);

            //    Assert.That(output, Is.EqualTo(input));
            //}

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
                using (var stream = new FileStream("4", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    list.Serialize(stream);
                }
                using (var stream = new FileStream("4", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                {
                    list.Deserialize(stream);
                }
                Assert.That(("(-1,'head')\n(1,'second')\n(4,'third')\n(0,'4th')\n(-1,'tail')"), Is.EqualTo(list.ToString()));
                
            }
        }
    }

}
