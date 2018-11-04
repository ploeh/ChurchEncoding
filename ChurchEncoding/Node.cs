using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public sealed class Node<T> : IBinaryTree<T>
    {
        private readonly T item;
        private readonly IBinaryTree<T> left;
        private readonly IBinaryTree<T> right;

        public Node(T item, IBinaryTree<T> left, IBinaryTree<T> right)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            this.item = item;
            this.left = left;
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

            return Equals(item, other.item)
                && Equals(left, other.left)
                && Equals(right, other.right);
        }

        public override int GetHashCode()
        {
            return item.GetHashCode() ^ left.GetHashCode() ^ right.GetHashCode();
        }
    }
}
