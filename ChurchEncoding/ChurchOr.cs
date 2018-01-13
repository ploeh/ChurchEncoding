using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public class ChurchOr : IChurchBoolean
    {
        private readonly IChurchBoolean x;
        private readonly IChurchBoolean y;

        public ChurchOr(IChurchBoolean x, IChurchBoolean y)
        {
            this.x = x;
            this.y = y;
        }

        public T Match<T>(ChurchBooleanParameters<T> parameters)
        {
            return x.Match(
                new ChurchBooleanParameters<T>(
                    parameters.TrueCase,
                    y.Match(parameters)));
        }
    }
}
