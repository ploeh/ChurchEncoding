using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public sealed class Node<T> : IBinaryTree<T>
    {
        private readonly IBinaryTree<T> left;
        private readonly T item;
        private readonly IBinaryTree<T> right;

        public Node(IBinaryTree<T> left, T item, IBinaryTree<T> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            this.left = left;
            this.item = item;
            this.right = right;
        }

        public TResult Match<TResult>(
            Func<TResult, T, TResult, TResult> node,
            Func<T, TResult> leaf)
        {
            return node(left.Match(node, leaf), item, right.Match(node, leaf));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Node<T> other))
                return false;

            return Equals(left, other.left)
                && Equals(item, other.item)
                && Equals(right, other.right);
        }

        public override int GetHashCode()
        {
            return left.GetHashCode() ^ item.GetHashCode() ^ right.GetHashCode();
        }
    }
}
