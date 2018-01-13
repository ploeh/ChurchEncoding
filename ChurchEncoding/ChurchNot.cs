using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    [DebuggerDisplay("{ b }")]
    public class ChurchNot : IChurchBoolean
    {
        private readonly IChurchBoolean b;

        public ChurchNot(IChurchBoolean b)
        {
            this.b = b;
        }

        public T Accept<T>(ChurchBooleanVisitor<T> visitor)
        {
            return b.Accept(
                new ChurchBooleanVisitor<T>(
                    trueCase  : visitor.VisitFalse,
                    falseCase : visitor.VisitTrue));
        }
    }
}
