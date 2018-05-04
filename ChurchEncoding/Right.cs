using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    [DebuggerDisplay("{ right }")]
    public sealed class Right<L, R> : IEither<L, R>
    {
        private readonly R right;

        public Right(R right)
        {
            this.right = right;
        }

        public T Accept<T>(IEitherVisitor<L, R, T> visitor)
        {
            return visitor.VisitRight(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Right<L, R> other))
                return false;

            return Equals(right, other.right);
        }

        public override int GetHashCode()
        {
            return right.GetHashCode();
        }
    }
}
