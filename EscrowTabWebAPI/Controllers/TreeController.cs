using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscrowTabWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreeController : Controller
    {
        private ILogger<TreeController> _logger;

        private readonly IMemoryCache _cache;

        private Tree _tree;

        private const string _KEY_CACHE_TREE = "KEY_TREE";

        private static int _index = 7;
        private static string NextIndex
        {
            get
            {
                ++_index;
                return _index.ToString();
            }
        }
        public TreeController(ILogger<TreeController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;

            _cache = memoryCache;

            _tree = new Tree();

            if (GetTreeFromCache() == null)
                // load the tree
                _tree.Root.Add(LoadTree());
            else
                // Get tree from the cache
                _tree.Root = GetTreeFromCache();

            // save tree to cache
            SaveTreeToCache(_tree.Root);
        }


        [HttpGet]
        public ActionResult GetTree()
        {
            var tree = GetTreeFromCache();

            return Ok(tree);
        }

        [HttpPost]
        public ActionResult AddNode([FromBody] RequestSaveNode request)
        {
            try
            {
                var tree = GetTreeFromCache();

                AddNode(tree[0], request);

                SaveTreeToCache(tree);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNode(string id)
        {
            try
            {
                var tree = GetTreeFromCache();

                DeleteNode(tree[0], tree[0], id);

                SaveTreeToCache(tree);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("{id}")]
        public ActionResult UpdateNode(string id, [FromBody] RequestUpdateNode request)
        {
            try
            {
                var tree = GetTreeFromCache();

                UpdateNode(tree[0], tree[0], request, id);

                SaveTreeToCache(tree);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
          
        }

        /// <summary>
        /// Add node to the list of children
        /// </summary>
        /// <param name="node"></param>
        /// <param name="req"></param>
        private void AddNode(Dictionary<string, Node> node, RequestSaveNode req)
        {
            if (node.ContainsKey(req.Parent))
            {
                Dictionary<string, Node> child = CreateNode(NextIndex, req.Label);
                node[req.Parent].Children.Add(child);
                return;
            }

            foreach (var child in node.Values.First().Children)
            {
                AddNode(child, req);
            }

            return;
        }

        /// <summary>
        /// Delete node of the corresponding id from the tree withough children
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        private void DeleteNode(Dictionary<string, Node> node, Dictionary<string, Node> parent, string id)
        {
            if (node.ContainsKey(id))
            {
                if (node[id].Children.Count == 0)
                {
                    parent.Values.First().Children.RemoveAll(x => x.ContainsKey(id));

                    return;
                }
            }

            for (int i = node.Values.First().Children.Count - 1; i >= 0; i--)
            {
                DeleteNode(node.Values.First().Children[i], node, id);
            }

            return;
        }

        /// <summary>
        /// Update the tree
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parent"></param>
        /// <param name="req"></param>
        /// <param name="parentId"></param>
        private void UpdateNode(Dictionary<string, Node> node, Dictionary<string, Node> parent, RequestUpdateNode req, string parentId)
        {
            if (node.ContainsKey(parentId))
            {
                // Create node
                Dictionary<string, Node> child = CreateNode(NextIndex, node[parentId].Label);

                // Add node to children
                node[parentId].Children.Add(child);

                // tempory node
                var tempNode = node[parentId];

                // remove current parent node
                parent.Values.First().Children.RemoveAll(x => x.ContainsKey(parentId));

                Dictionary<string, Node> item = new Dictionary<string, Node>();

                item.Add(req.NewParentId.ToString(), tempNode);

                parent.Values.First().Children.Add(item);

                return;
            }

            for (int i = node.Values.First().Children.Count - 1; i >= 0; i--)
            {
                UpdateNode(node.Values.First().Children[i], node, req, parentId);
            }

            return;

        }

        private List<Dictionary<string, Node>> GetTreeFromCache()
        {
            List<Dictionary<string, Node>> root = null;

            _cache.TryGetValue(_KEY_CACHE_TREE, out root);

            return root;
        }

        private void SaveTreeToCache(List<Dictionary<string, Node>> root)
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(60),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(1),
                Size = 1024,
            };

            _cache.Set(_KEY_CACHE_TREE, root, cacheExpiryOptions);
        }

        private Dictionary<string, Node> LoadTree()
        {
            Dictionary<string, Node> root = CreateNode("1", "root");
            Dictionary<string, Node> ant = CreateNode("2", "ant");
            Dictionary<string, Node> bear = CreateNode("3", "bear");
            Dictionary<string, Node> cat = CreateNode("4", "cat");
            Dictionary<string, Node> dog = CreateNode("5", "dog");
            Dictionary<string, Node> elephant = CreateNode("6", "elephan");
            Dictionary<string, Node> frog = CreateNode("7", "frog");

            root["1"].Children.Add(ant);
            root["1"].Children.Add(bear);
            root["1"].Children.Add(frog);
            bear["3"].Children.Add(cat);
            bear["3"].Children.Add(dog);
            dog["5"].Children.Add(elephant);

            return root;
        }

        private static Dictionary<string, Node> CreateNode(string key, string label)
        {
            Dictionary<string, Node> node = new Dictionary<string, Node>();
            node.Add(key, new Node() { Label = label, Children = new List<Dictionary<string, Node>>() });
            return node;
        }

    }
}
