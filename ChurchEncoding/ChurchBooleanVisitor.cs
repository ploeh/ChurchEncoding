using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchBooleanVisitor<T>
    {
        public ChurchBooleanVisitor(T trueCase, T falseCase)
        {
            VisitTrue = trueCase;
            VisitFalse = falseCase;
        }

        public T VisitTrue { get; }
        public T VisitFalse { get; }
    }
}
