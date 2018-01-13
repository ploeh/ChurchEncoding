using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class Zero : INaturalNumber
    {
        public T Match<T>(NaturalNumberParameters<T> parameters)
        {
            return parameters.Zero;
        }
    }
}
