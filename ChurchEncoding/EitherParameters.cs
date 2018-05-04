using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class EitherParameters<L, R, T>
    {
        public EitherParameters(
            Func<L, T> runLeft,
            Func<R, T> runRight)
        {
            RunLeft = runLeft;
            RunRight = runRight;
        }

        public Func<L, T> RunLeft { get;  }
        public Func<R, T> RunRight { get; }
    }
}
