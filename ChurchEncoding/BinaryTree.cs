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
            T item,
            IBinaryTree<T> left,
            IBinaryTree<T> right)
        {
            return new Node<T>(item, left, right);
        }

        public static IBinaryTree<TResult> Select<TResult, T>(
            this IBinaryTree<T> tree,
            Func<T, TResult> selector)
        {
            if (tree == null)
                throw new ArgumentNullException(nameof(tree));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var visitor = new SelectBinaryTreeVisitor<T, TResult>(selector);
            return tree.Accept(visitor);
        }

        private class SelectBinaryTreeVisitor<T, TResult> :
            IBinaryTreeVisitor<T, IBinaryTree<TResult>>
        {
            private readonly Func<T, TResult> selector;

            public SelectBinaryTreeVisitor(Func<T, TResult> selector)
            {
                if (selector == null)
                    throw new ArgumentNullException(nameof(selector));

                this.selector = selector;
            }

            public IBinaryTree<TResult> Visit(Leaf<T> leaf)
            {
                var mappedItem = selector(leaf.Item);
                return Leaf(mappedItem);
            }

            public IBinaryTree<TResult> Visit(Node<T> node)
            {
                var mappedItem = selector(node.Item);
                var mappedLeft = node.Left.Accept(this);
                var mappedRight = node.Right.Accept(this);
                return Create(mappedItem, mappedLeft, mappedRight);
            }
        }
    }
}
