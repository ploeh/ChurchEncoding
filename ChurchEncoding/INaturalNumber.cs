using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface INaturalNumber
    {
        T Match<T>(INaturalNumberParameters<T> parameters);
    }
}
