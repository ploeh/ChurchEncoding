using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public static class Either
    {
        // Bifunctor
        public static IEither<L1, R1> SelectBoth<L, L1, R, R1>(
            this IEither<L, R> source,
            Func<L, L1> selectLeft,
            Func<R, R1> selectRight)
        {
            return source.Match<IEither<L1, R1>>(
                onLeft:  l => new Left<L1, R1>(selectLeft(l)),
                onRight: r => new Right<L1, R1>(selectRight(r)));
        }

        public static IEither<L1, R> SelectLeft<L, L1, R>(
            this IEither<L, R> source,
            Func<L, L1> selector)
        {
            return source.SelectBoth(selector, r => r);
        }

        public static IEither<L, R1> SelectRight<L, R, R1>(
            this IEither<L, R> source,
            Func<R, R1> selector)
        {
            return source.SelectBoth(l => l, selector);
        }
    }
}
