using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IEitherParameters<L, R, T>
    {
        T RunLeft(L left);
        T RunRight(R right);
    }
}
