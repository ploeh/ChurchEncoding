using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public sealed class Leaf<T> : IBinaryTree<T>
    {
        public T Item { get; }

        public Leaf(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Item = item;
        }

        public TResult Accept<TResult>(BinaryTreeVisitor<T, TResult> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            return visitor.VisitLeaf(this);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Leaf<T> other))
                return false;

            return Equals(Item, other.Item);
        }

        public override int GetHashCode()
        {
            return Item.GetHashCode();
        }
    }
}
