using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IRoseTreeVisitor<N, L, T>
    {
        T VisitNode(N node, IEnumerable<IRoseTree<N, L>> branches);
        T VisitLeaf(L leaf);
    }
}
