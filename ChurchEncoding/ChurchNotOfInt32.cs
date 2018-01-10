using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchNotOfInt32 : IChurchBooleanOfInt32
    {
        private readonly IChurchBooleanOfInt32 b;

        public ChurchNotOfInt32(IChurchBooleanOfInt32 b)
        {
            this.b = b;
        }

        public int Match(int trueCase, int falseCase)
        {
            return this.b.Match(falseCase, trueCase);
        }
    }
}
