using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    [DebuggerDisplay("{ value }")]
    public sealed class RoseLeaf<N, L> : IRoseTree<N, L>
    {
        private readonly L value;

        public RoseLeaf(L value)
        {
            this.value = value;
        }

        public TResult Accept<TResult>(
            IRoseTreeVisitor<N, L, TResult> visitor)
        {
            return visitor.VisitLeaf(value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoseLeaf<N, L> other))
                return false;

            return Equals(value, other.value);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
