using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public sealed class Leaf<T> : IBinaryTree<T>
    {
        private readonly T item;

        public Leaf(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            this.item = item;
        }

        public TResult Match<TResult>(
            Func<Node<T>, TResult> node,
            Func<T, TResult> leaf)
        {
            return leaf(item);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Leaf<T> other))
                return false;

            return Equals(item, other.item);
        }

        public override int GetHashCode()
        {
            return item.GetHashCode();
        }
    }
}
