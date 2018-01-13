using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    [DebuggerDisplay("{ value }")]
    public class Just<T> : IMaybe<T>
    {
        private readonly T value;

        public Just(T value)
        {
            this.value = value;
        }

        public TResult Accept<TResult>(IMaybeVisitor<T, TResult> visitor)
        {
            return visitor.VisitJust(value);
        }
    }
}
