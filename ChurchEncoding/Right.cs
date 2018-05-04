using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    [DebuggerDisplay("{ right }")]
    public class Right<L, R> : IEither<L, R>
    {
        private readonly R right;

        public Right(R right)
        {
            this.right = right;
        }

        public T Match<T>(Func<L, T> onLeft, Func<R, T> onRight)
        {
            return onRight(right);
        }
    }
}
