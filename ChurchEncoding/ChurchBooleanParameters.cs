using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchBooleanParameters<T>
    {
        public ChurchBooleanParameters(T trueCase, T falseCase)
        {
            TrueCase = trueCase;
            FalseCase = falseCase;
        }

        public T TrueCase { get; }
        public T FalseCase { get; }
    }
}
