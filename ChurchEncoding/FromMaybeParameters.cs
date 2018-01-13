using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    // Does the same work as Haskell's Data.Maybe.fromMaybe function, hence the
    // name.
    public class FromMaybeParameters<T> : IMaybeParameters<T, T>
    {
        private readonly T nothingResult;

        public FromMaybeParameters(T nothingResult)
        {
            this.nothingResult = nothingResult;
        }

        public T Nothing
        {
            get { return nothingResult; }
        }

        public T Just(T just)
        {
            return just;
        }
    }
}
