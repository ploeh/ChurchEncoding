using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class BinaryTreeVisitor<T, TResult>
    {
        public BinaryTreeVisitor(
            Func<Node<T>, TResult> visitNode,
            Func<Leaf<T>, TResult> visitLeaf)
        {
            VisitNode = visitNode;
            VisitLeaf = visitLeaf;
        }

        public Func<Node<T>, TResult> VisitNode { get; }

        public Func<Leaf<T>, TResult> VisitLeaf { get; }
    }
}
