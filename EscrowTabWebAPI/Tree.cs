using System.Collections.Generic;

namespace EscrowTabWebAPI
{
    public class Tree
    {
        public List<Dictionary<string, Node>> Root { get ; set; }

        public Tree() {
           Root = new List<Dictionary<string, Node>>();
        }
      
    }
}
