using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IBinaryTree<T>
    {
        TResult Match<TResult>(
            Func<TResult, T, TResult, TResult> node,
            Func<T, TResult> leaf);
    }
}
