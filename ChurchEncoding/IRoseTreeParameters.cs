using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IRoseTreeParameters<N, L, T>
    {
        T RunNode(N node, IEnumerable<IRoseTree<N, L>> branches);
        T RunLeaf(L leaf);
    }
}
