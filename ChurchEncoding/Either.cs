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
            return source.Match(
                new SelectBothParameters<L, L1, R, R1>(selectLeft, selectRight));
        }

        private class SelectBothParameters<L, L1, R, R1> :
            IEitherParameters<L, R, IEither<L1, R1>>
        {
            private readonly Func<L, L1> selectLeft;
            private readonly Func<R, R1> selectRight;

            public SelectBothParameters(
                Func<L, L1> selectLeft,
                Func<R, R1> selectRight)
            {
                this.selectLeft = selectLeft;
                this.selectRight = selectRight;
            }

            public IEither<L1, R1> RunLeft(L left)
            {
                return new Left<L1, R1>(selectLeft(left));
            }

            public IEither<L1, R1> RunRight(R right)
            {
                return new Right<L1, R1>(selectRight(right));
            }
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
            return source.Match(new JoinParameters<L, R>());
        }

        private class JoinParameters<L, R> :
            IEitherParameters<L, IEither<L, R>, IEither<L, R>>
        {
            public IEither<L, R> RunLeft(L left)
            {
                return new Left<L, R>(left);
            }

            public IEither<L, R> RunRight(IEither<L, R> right)
            {
                return right;
            }
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

        // Bifoldable - sort of... Really, the T involved should give rise to a
        // monoid
        public static T Bifold<T>(this IEither<T, T> source)
        {
            return source.Match(new BifoldParameters<T>());
        }

        private class BifoldParameters<T> : IEitherParameters<T, T, T>
        {
            public T RunLeft(T left)
            {
                return left;
            }

            public T RunRight(T right)
            {
                return right;
            }
        }
    }
}
