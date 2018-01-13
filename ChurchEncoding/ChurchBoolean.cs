using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public static class ChurchBoolean
    {
        public static bool ToBool(this IChurchBoolean b)
        {
            return b.Accept(new ChurchBooleanVisitor<bool>(true, false));
        }

        public static IChurchBoolean ToChurchBoolean(this bool b)
        {
            if (b)
                return new ChurchTrue();
            else
                return new ChurchFalse();
        }
    }
}
