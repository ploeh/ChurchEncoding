using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchTrue : IChurchBoolean
    {
        public T Match<T>(T trueCase, T falseCase)
        {
            return trueCase;
        }
    }
}
