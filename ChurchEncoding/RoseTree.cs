using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public static class RoseTree
    {
        public static IRoseTree<N, L> Node<N, L>(
            N value,
            params IRoseTree<N, L>[] branches)
        {
            return new RoseNode<N, L>(value, branches);
        }

        public static IChurchBoolean IsLeaf<N, L>(this IRoseTree<N, L> source)
        {
            return source.Match(new IsLeafParameters<N, L>());
        }

        private class IsLeafParameters<N, L> :
            IRoseTreeParameters<N, L, IChurchBoolean>
        {
            public IChurchBoolean RunLeaf(L leaf)
            {
                return new ChurchTrue();
            }

            public IChurchBoolean RunNode(
                N node, IEnumerable<IRoseTree<N, L>> branches)
            {
                return new ChurchFalse();
            }
        }

        public static IChurchBoolean IsNode<N, L>(this IRoseTree<N, L> source)
        {
            return new ChurchNot(source.IsLeaf());
        }

        public static TResult Cata<N, L, TResult>(
            this IRoseTree<N, L> tree,
            Func<N, IEnumerable<TResult>, TResult> node,
            Func<L, TResult> leaf)
        {
            return tree.Match(new CataParameters<N, L, TResult>(node, leaf));
        }

        private class CataParameters<N, L, TResult> : IRoseTreeParameters<N, L, TResult>
        {
            private readonly Func<N, IEnumerable<TResult>, TResult> node;
            private readonly Func<L, TResult> leaf;

            public CataParameters(
                Func<N, IEnumerable<TResult>, TResult> node,
                Func<L, TResult> leaf)
            {
                this.node = node;
                this.leaf = leaf;
            }

            public TResult RunLeaf(L leaf)
            {
                return this.leaf(leaf);
            }

            public TResult RunNode(N node, IEnumerable<IRoseTree<N, L>> branches)
            {
                return this.node(
                    node,
                    branches.Select(t => t.Cata(this.node, leaf)));
            }
        }

        // Bifunctor
        public static IRoseTree<N1, L1> SelectBoth<N, N1, L, L1>(
            this IRoseTree<N, L> source,
            Func<N, N1> selectNode,
            Func<L, L1> selectLeaf)
        {
            return source.Cata(
                node: (n, branches) => new RoseNode<N1, L1>(selectNode(n), branches),
                leaf: l => (IRoseTree<N1, L1>)new RoseLeaf<N1, L1>(selectLeaf(l)));
        }

        public static IRoseTree<N1, L> SelectNode<N, N1, L>(
            this IRoseTree<N, L> source,
            Func<N, N1> selector)
        {
            return source.SelectBoth(selector, l => l);
        }

        public static IRoseTree<N, L1> SelectLeaf<N, L, L1>(
            this IRoseTree<N, L> source,
            Func<L, L1> selector)
        {
            return source.SelectBoth(n => n, selector);
        }

        // Functor
        public static IRoseTree<N, L1> Select<N, L, L1>(
            this IRoseTree<N, L> source,
            Func<L, L1> selector)
        {
            return source.SelectLeaf(selector);
        }
    }
}
