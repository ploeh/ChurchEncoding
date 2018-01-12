using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class MaybeParameters<T, TResult>
    {
        public MaybeParameters(TResult nothing, Func<T, TResult> just)
        {
            this.Nothing = nothing;
            this.Just = just;
        }

        public TResult Nothing { get; }

        public Func<T, TResult> Just { get; }
    }
}
