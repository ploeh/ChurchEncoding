using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IMaybe<T>
    {
        TResult Accept<TResult>(IMaybeVisitor<T, TResult> visitor);
    }
}
