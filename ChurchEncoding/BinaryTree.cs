using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public static class BinaryTree
    {
        public static IBinaryTree<T> Leaf<T>(T item)
        {
            return new Leaf<T>(item);
        }

        public static IBinaryTree<T> Create<T>(
            IBinaryTree<T> left,
            T item,
            IBinaryTree<T> right)
        {
            return new Node<T>(left, item, right);
        }

        public static IBinaryTree<TResult> Select<TResult, T>(
            this IBinaryTree<T> tree,
            Func<T, TResult> selector)
        {
            if (tree == null)
                throw new ArgumentNullException(nameof(tree));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return tree.Match(
                node: (l, x, r) => Create(l, selector(x), r),
                leaf: x => Leaf(selector(x)));
        }

        public static int Sum(this IBinaryTree<int> tree)
        {
            return tree.Match((l, x, r) => l + x + r, x => x);
        }

        public static int Max(this IBinaryTree<int> tree)
        {
            return tree.Match((l, x, r) => Math.Max(Math.Max(l, r), x), x => x);
        }

        public static int CountLeaves<T>(this IBinaryTree<T> tree)
        {
            return tree.Match((l, _, r) => l + r, _ => 1);
        }

        public static int MeasureDepth<T>(this IBinaryTree<T> tree)
        {
            return tree.Match((l, _, r) => 1 + Math.Max(l, r), _ => 0);
        }
    }
}
