using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IMaybeParameters<T, TResult>
    {
        TResult Nothing { get; }

        TResult Just(T just);
    }
}
