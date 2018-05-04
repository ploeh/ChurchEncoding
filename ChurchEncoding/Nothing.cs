using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class Nothing<T> : IMaybe<T>
    {
        public TResult Match<TResult>(TResult nothing, Func<T, TResult> just)
        {
            return nothing;
        }
    }
}
