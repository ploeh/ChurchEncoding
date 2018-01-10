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

        public object Match(object trueCase, object falseCase)
        {
            return b.Match(falseCase, trueCase);
        }
    }
}
