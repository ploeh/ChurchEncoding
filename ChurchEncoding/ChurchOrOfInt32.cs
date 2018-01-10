using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchOrOfInt32 : IChurchBooleanOfInt32
    {
        private readonly IChurchBooleanOfInt32 x;
        private readonly IChurchBooleanOfInt32 y;

        public ChurchOrOfInt32(IChurchBooleanOfInt32 x, IChurchBooleanOfInt32 y)
        {
            this.x = x;
            this.y = y;
        }

        public int Match(int trueCase, int falseCase)
        {
            return this.x.Match(trueCase, y.Match(trueCase, falseCase));
        }
    }
}
