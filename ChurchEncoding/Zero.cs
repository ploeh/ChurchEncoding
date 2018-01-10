using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class Zero : INaturalNumber
    {
        public T Match<T>(T zero, Func<INaturalNumber, T> succ)
        {
            return zero;
        }
    }
}
