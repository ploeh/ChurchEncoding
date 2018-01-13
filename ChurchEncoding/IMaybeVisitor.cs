using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IMaybeVisitor<T, TResult>
    {
        TResult VisitNothing { get; }

        TResult VisitJust(T just);
    }
}
