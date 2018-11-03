using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IBinaryTreeVisitor<T, TResult>
    {
        TResult Visit(Node<T> node);

        TResult Visit(Leaf<T> leaf);
    }
}
