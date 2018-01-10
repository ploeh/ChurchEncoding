using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchTrueOfString : IChurchBooleanOfString
    {
        public string Match(string trueCase, string falseCase)
        {
            return trueCase;
        }
    }
}
