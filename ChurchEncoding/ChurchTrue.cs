using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchTrue : IChurchBoolean
    {
        public T Accept<T>(ChurchBooleanVisitor<T> visitor)
        {
            return visitor.VisitTrue;
        }
    }
}
