using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public sealed class Node<T> : IBinaryTree<T>
    {
        public T Item { get; }
        public IBinaryTree<T> Left { get; }
        public IBinaryTree<T> Right { get; }

        public Node(T item, IBinaryTree<T> left, IBinaryTree<T> right)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            Item = item;
            Left = left;
            Right = right;
        }

        public TResult Match<TResult>(
            Func<Node<T>, TResult> node,
            Func<T, TResult> leaf)
        {
            return node(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Node<T> other))
                return false;

            return Equals(Item, other.Item)
                && Equals(Left, other.Left)
                && Equals(Right, other.Right);
        }

        public override int GetHashCode()
        {
            return Item.GetHashCode() ^ Left.GetHashCode() ^ Right.GetHashCode();
        }
    }
}
