using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class Zero : INaturalNumber
    {
        public T Accept<T>(INaturalNumberVisitor<T> visitor)
        {
            return visitor.VisitZero;
        }
    }
}
