using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class NaturalNumberParameters<T>
    {
        public NaturalNumberParameters(T zero, Func<INaturalNumber, T> succ)
        {
            this.Zero = zero;
            this.Succ = succ;
        }

        public T Zero { get; }
        public Func<INaturalNumber, T> Succ { get; }
    }
}
