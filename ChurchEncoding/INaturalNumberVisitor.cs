using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface INaturalNumberVisitor<T>
    {
        T VisitZero { get; }
        T VisitSucc(INaturalNumber predecessor);
    }
}
