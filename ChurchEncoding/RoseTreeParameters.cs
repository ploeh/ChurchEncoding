using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class RoseTreeParameters<N, L, T>
    {
        public RoseTreeParameters(
            Func<N, IEnumerable<IRoseTree<N, L>>, T> node,
            Func<L, T> leaf)
        {
            Node = node;
            Leaf = leaf;
        }

        public Func<N, IEnumerable<IRoseTree<N, L>>, T> Node { get; }
        public Func<L, T> Leaf { get; }
    }
}
