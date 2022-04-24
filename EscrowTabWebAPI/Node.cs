using System.Collections.Generic;

namespace EscrowTabWebAPI
{
    public class Node
    {
        public string Label { get; set; }

        public List<Dictionary<string, Node>> Children { get; set; }
    }
}