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

        // Functor
        public static IEither<L, R1> Select<L, R, R1>(
            this IEither<L, R> source,
            Func<R, R1> selector)
        {
            return source.SelectRight(selector);
        }

        // Monad
        public static IEither<L, R> Join<L, R>(
            this IEither<L, IEither<L, R>> source)
        {
            return source.Match(
                onLeft:  l => new Left<L, R>(l),
                onRight: r => r);
        }

        public static IEither<L, R1> SelectMany<L, R, R1>(
            this IEither<L, R> source,
            Func<R, IEither<L, R1>> selector)
        {
            return source.Select(selector).Join();
        }

        public static IEither<L, R1> SelectMany<L, R, R1, U>(
            this IEither<L, R> source,
            Func<R, IEither<L, U>> k,
            Func<R, U, R1> s)
        {
            return source
                .SelectMany(x => k(x)
                    .SelectMany(y => new Right<L, R1>(s(x, y))));
        }
    }
}
