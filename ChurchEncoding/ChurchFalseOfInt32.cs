using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchFalseOfInt32 : IChurchBooleanOfInt32
    {
        public int Match(int trueCase, int falseCase)
        {
            return falseCase;
        }
    }
}
