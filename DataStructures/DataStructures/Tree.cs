using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures {
    public class Tree<T> {
        private T data;
        private Tree<T> parent;
        private List<Tree<T>> children;

        public T Data {
            get {
                return data;
            }
        }

        public List<Tree<T>> Children {
            get {
                return children;
            }
        }

        public Tree(T data, Tree<T> parent = null) {
            this.data = data;
            this.parent = parent;
            this.children = new List<Tree<T>>();
        }

        public Tree<T> AddChild(T data) {
            var node = new Tree<T>(data, this);
            this.children.Add(node);
            return node;
        }

        public static Tree<T> Find(Tree<T> node, Func<Tree<T>, bool> predicate) {
            Tree<T> foundNode;
            if(predicate(node)){
                return node;
            }
            foundNode = node.Children.Where(c => predicate(c)).FirstOrDefault();
            if (foundNode != null) {
                return foundNode;
            } else {
                foreach (var child in node.Children) {
                    foundNode = Find(child, predicate);
                    if (foundNode != null) return foundNode;
                }
            }
            return foundNode;
        }
    }
}
