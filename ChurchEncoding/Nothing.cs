using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class Nothing<T> : IMaybe<T>
    {
        public TResult Match<TResult>(IMaybeParameters<T, TResult> parameters)
        {
            return parameters.Nothing;
        }
    }
}
