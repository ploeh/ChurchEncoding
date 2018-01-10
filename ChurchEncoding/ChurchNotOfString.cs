using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchNotOfString : IChurchBooleanOfString
    {
        private readonly IChurchBooleanOfString b;

        public ChurchNotOfString(IChurchBooleanOfString b)
        {
            this.b = b;
        }

        public string Match(string trueCase, string falseCase)
        {
            return this.b.Match(falseCase, trueCase);
        }
    }
}
