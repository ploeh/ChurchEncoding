using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IRoseTree<N, L>
    {
        TResult Match<TResult>(
            Func<N, IEnumerable<IRoseTree<N, L>>, TResult> node,
            Func<L, TResult> leaf);
    }
}
