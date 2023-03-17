using System.Text;
using System.Text.RegularExpressions;

class ListNode
{
    public ListNode Prev;
    public ListNode Next;
    public ListNode Rand; // произвольный элемент внутри списка
    public string Data;

    public ListNode(ListNode rand, string data) //for tests
    {
        Rand = rand;
        Data = data;
    }

    public ListNode() : this(null, null) { }

    public void ConnectToPrev(ListNode other)
    {
        this.Prev = other;
        other.Next = this;
    }

    public override string ToString()
    {
        return $"Prev: {this.Prev}, Next: {this.Next}, Rand: {this.Rand}, Data: {this.Data}";
    }
}

class ListRand
{
    public ListNode Head;
    public ListNode Tail;
    public int Count;

    public ListRand(ListNode head, ListNode tail, int count) //for tests
    {
        Head = head;
        Tail = tail;
        Count = count;
    }

    public override string ToString()
    {
        var numberByNode = GetOrderedDictionary();
        string listNodeInformation = ConvertToString(numberByNode);
        return listNodeInformation;
    }
    public void Serialize(FileStream s)
    {
        var numberByNode = GetOrderedDictionary();
        string listNodeInformation = ConvertToString(numberByNode);
        byte[] encodedInformation = Encoding.ASCII.GetBytes(listNodeInformation.ToString());
        s.Write(encodedInformation);
    }

    public void Deserialize(FileStream s)
    {
        var nodes = CreateNodes();
        string decodedLine = DecodeBytes(s);
        AnalyzeLine(decodedLine, nodes);
    }

    public Dictionary<ListNode, int> GetOrderedDictionary()
    {
        var dictionary = new Dictionary<ListNode, int>();
        int index = 0;
        foreach (var item in Nodes())
        {
            dictionary[item] = index++;
        }
        return dictionary;
    }

    public string ConvertToString(Dictionary<ListNode,int> dictionary)
    {
        var output = new StringBuilder();
        foreach (var node in Nodes())
        {
            var rand = node.Rand;
            int number = rand == null ? -1 : dictionary[rand];
            string nodeInformation = $"{number},'{node.Data}'";
            output.Append("(" + nodeInformation + ")\n");
        }
        output.Length = output.Length - 1;
        return output.ToString();
    }

    public ListNode[] CreateNodes()
    {
        var nodes = new ListNode[Count]; 
        nodes[0] = Head;
        nodes[Count - 1] = Tail;

        var prevNode = Head;
        for (int i = 1; i < Count - 1; i++)
        {
            var current = new ListNode();
            nodes[i] = current;
            current.ConnectToPrev(prevNode);
            prevNode = current;
        }
        Tail.ConnectToPrev(prevNode);
        return nodes;
    }

    public string DecodeBytes(FileStream s)
    {
        byte[] encodedInformation = File.ReadAllBytes(s.Name);
        string decoded = Encoding.ASCII.GetString(encodedInformation);
        return decoded;
    }

    public void AnalyzeLine(string line, ListNode[] nodes)
    {
        string pattern = @"(?<=((?<Rand>-*\d),'))(?<Data>.*)(?=('\)))";
        var mathes = Regex.Matches(line, pattern, RegexOptions.ExplicitCapture);
        int index = 0;
        foreach (Match matchItem in mathes)
        {
            var randValue = matchItem.Groups["Rand"].Value;
            int randNumber = Int32.Parse(randValue);
            string data = matchItem.Groups["Data"].Value.Replace(@"\n", "\n");

            var current = nodes[index++];
            current.Rand = randNumber != -1 ? nodes[randNumber] : null;
            current.Data = data;
        }
    }

    public IEnumerable<ListNode> Nodes()
    {
        var value = Head;
        while (value != null)
        {
            yield return value;
            value = value.Next;
        }
    }
}


