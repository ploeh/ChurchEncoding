using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    [DebuggerDisplay("{ left }")]
    public sealed class Left<L, R> : IEither<L, R>
    {
        private readonly L left;

        public Left(L left)
        {
            this.left = left;
        }

        public T Match<T>(EitherParameters<L, R, T> parameters)
        {
            return parameters.RunLeft(left);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Left<L, R> other))
                return false;

            return Equals(left, other.left);
        }

        public override int GetHashCode()
        {
            return left.GetHashCode();
        }
    }
}
