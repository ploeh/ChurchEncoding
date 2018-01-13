using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchFalse : IChurchBoolean
    {
        public T Match<T>(ChurchBooleanParameters<T> parameters)
        {
            return parameters.FalseCase;
        }
    }
}
