using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    [DebuggerDisplay("{ left }")]
    public class Left<L, R> : IEither<L, R>
    {
        private readonly L left;

        public Left(L left)
        {
            this.left = left;
        }

        public T Match<T>(Func<L, T> onLeft, Func<R, T> onRight)
        {
            return onLeft(left);
        }
    }
}
