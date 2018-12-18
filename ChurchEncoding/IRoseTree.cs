using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IRoseTree<N, L>
    {
        TResult Accept<TResult>(IRoseTreeVisitor<N, L, TResult> visitor);
    }
}
