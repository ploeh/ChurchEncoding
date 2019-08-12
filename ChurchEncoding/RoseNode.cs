using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    [DebuggerDisplay("{ value }, { branches }")]
    public sealed class RoseNode<N, L> : IRoseTree<N, L>
    {
        private readonly N value;
        private readonly IEnumerable<IRoseTree<N, L>> branches;

        public RoseNode(N value, IEnumerable<IRoseTree<N, L>> branches)
        {
            this.value = value;
            this.branches = branches;
        }

        public TResult Accept<TResult>(
            IRoseTreeVisitor<N, L, TResult> visitor)
        {
            return visitor.VisitNode(value, branches);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoseNode<N, L> other))
                return false;

            return Equals(value, other.value)
                && Enumerable.SequenceEqual(branches, other.branches);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode() ^ branches.GetHashCode();
        }
    }
}
