using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IEitherVisitor<L, R, T>
    {
        T VisitLeft(L left);
        T VisitRight(R right);
    }
}
