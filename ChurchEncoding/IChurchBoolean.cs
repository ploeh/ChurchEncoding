﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public interface IChurchBoolean
    {
        T Accept<T>(ChurchBooleanVisitor<T> visitor);
    }
}
