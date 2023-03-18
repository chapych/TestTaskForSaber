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
        var output = new StringBuilder();
        foreach (var node in Nodes())
        {
            var rand = node.Rand;
            int number = rand == null ? -1 : numberByNode[rand];
            string nodeInformation = $"{number},'{node.Data}'";
            output.Append("(" + nodeInformation + ")\n");
        }
        output.Length = output.Length - 1;
        return output.ToString();
    }
    public void Serialize(FileStream s)
    {
        StreamWriter sw = new StreamWriter(s);
        var numberByNode = GetOrderedDictionary();
        foreach(var node in Nodes())
        {
            string nodeInformation = ConvertToString(numberByNode, node);
            byte[] encodedInformation = Encoding.ASCII.GetBytes(nodeInformation.ToString()+"\n");
            s.Write(encodedInformation);
        }
        
    }

    public void Deserialize(FileStream s)
    {
        var nodes = CreateNodes();
        StreamReader sr = new StreamReader(s);
        int index = 0;
        while (!sr.EndOfStream)
        {
            string current = sr.ReadLine();
            AnalyzeLine(current, nodes, index++);
        }
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

    public string ConvertToString(Dictionary<ListNode,int> dictionary, ListNode node)
    {
        var output = new StringBuilder();
        var rand = node.Rand;
        int number = rand == null ? -1 : dictionary[rand];
        string nodeInformation = $"({number},'{node.Data}')";
        return nodeInformation;
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

    public void AnalyzeLine(string line, ListNode[] nodes, int index)
    {
        //int randNumber = 0;
        //string data = string.Empty;
        FindOrderAndData(line, out int randNumber, out string data);
        var current = nodes[index];
        current.Rand = randNumber != -1 ? nodes[randNumber] : null;
        current.Data = data;
        
    }

    public static void FindOrderAndData(string line, out int randNumber, out string data)
    {
        string pattern = @"(?<=((?<Rand>-*\d),'))(?<Data>.*)(?=('\)))";
        var match = Regex.Match(line, pattern, RegexOptions.ExplicitCapture);

        var randValue = match.Groups["Rand"].Value;
        randNumber = Int32.Parse(randValue);
        data = match.Groups["Data"].Value.Replace("\\n", "\n"); //in case if data contains new line
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

