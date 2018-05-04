using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class Just<T> : IMaybe<T>
    {
        private readonly T value;

        public Just(T value)
        {
            this.value = value;
        }

        public TResult Match<TResult>(TResult nothing, Func<T, TResult> just)
        {
            return just(value);
        }
    }
}
