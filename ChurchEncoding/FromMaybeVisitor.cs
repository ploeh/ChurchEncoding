using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    // Does the same work as Haskell's Data.Maybe.fromMaybe function, hence the
    // name.
    public class FromMaybeVisitor<T> : IMaybeVisitor<T, T>
    {
        private readonly T nothingResult;

        public FromMaybeVisitor(T nothingResult)
        {
            this.nothingResult = nothingResult;
        }

        public T VisitNothing
        {
            get { return nothingResult; }
        }

        public T VisitJust(T just)
        {
            return just;
        }
    }
}
