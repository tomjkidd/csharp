using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures {
    public class TreeUsage {
        public static void TreeUsageMain() {
            var tree = CreateTree();

            var json = JsonConvert.SerializeObject(tree, Formatting.Indented, new JsonSerializerSettings() {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            });

            Console.Write(json);
            Console.ReadLine();
        }

        public static Tree<FlatData> CreateTree() {
            List<FlatData> data = new List<FlatData>() {
                new FlatData() { ParentId = null, Id = "A", Data = "AData" },
                new FlatData() { ParentId = "A", Id = "B", Data = "BData" },
                new FlatData() { ParentId = "A", Id = "C", Data = "CData" },
                new FlatData() { ParentId = "C", Id = "D", Data = "DData" },
                new FlatData() { ParentId = "B", Id = "E", Data = "EData" }
            };

            // Make sure there is only a single root for the tree, this could be relaxed with more effort.
            Trace.Assert(data.Where(n => n.ParentId == null).Count() == 1);

            Tree<FlatData> root = new Tree<FlatData>(data.Where(n => n.ParentId == null).FirstOrDefault());
            Tree<FlatData> parent;
            foreach (FlatData n in data) {
                if (n.ParentId != null) {
                    parent = Tree<FlatData>.Find(root, current => current.Data.Id == n.ParentId);
                    parent.AddChild(n);
                }
            }

            return root;
        }       
    }

    public class FlatData {
        public string ParentId { get; set; }
        public string Id { get; set; }
        public string Data { get; set; }
    }
}
