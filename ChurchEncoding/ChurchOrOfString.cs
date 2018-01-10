using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchOrOfString : IChurchBooleanOfString
    {
        private readonly IChurchBooleanOfString x;
        private readonly IChurchBooleanOfString y;

        public ChurchOrOfString(IChurchBooleanOfString x, IChurchBooleanOfString y)
        {
            this.x = x;
            this.y = y;
        }

        public string Match(string trueCase, string falseCase)
        {
            return this.x.Match(trueCase, y.Match(trueCase, falseCase));
        }
    }
}
