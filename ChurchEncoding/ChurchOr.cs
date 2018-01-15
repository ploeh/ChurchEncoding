using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchOr : IChurchBoolean
    {
        private readonly IChurchBoolean x;
        private readonly IChurchBoolean y;

        public ChurchOr(IChurchBoolean x, IChurchBoolean y)
        {
            this.x = x;
            this.y = y;
        }

        public object Match(object trueCase, object falseCase)
        {
            return x.Match(trueCase, y.Match(trueCase, falseCase));
        }

        public int Match(int trueCase, int falseCase)
        {
            return this.x.Match(trueCase, y.Match(trueCase, falseCase));
        }

        public string Match(string trueCase, string falseCase)
        {
            return this.x.Match(trueCase, y.Match(trueCase, falseCase));
        }
    }
}
