using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IBinaryTree<T>
    {
        TResult Accept<TResult>(
            Func<Node<T>, TResult> visitNode,
            Func<Leaf<T>, TResult> visitLeaf);
    }
}
