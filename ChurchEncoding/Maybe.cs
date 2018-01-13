using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public static class Maybe
    {
        public static IChurchBoolean IsNothing<T>(this IMaybe<T> m)
        {
            return m.Match(new IsNothingMaybeParameters<T>());
        }

        private class IsNothingMaybeParameters<T> :
            IMaybeParameters<T, IChurchBoolean>
        {
            public IChurchBoolean Nothing
            {
                get { return new ChurchTrue(); }
            }

            public IChurchBoolean Just(T just)
            {
                return new ChurchFalse();
            }
        }

        public static IChurchBoolean IsJust<T>(this IMaybe<T> m)
        {
            return m.Match(new IsJustMaybeParameters<T>());
        }

        private class IsJustMaybeParameters<T> :
            IMaybeParameters<T, IChurchBoolean>
        {
            public IChurchBoolean Nothing
            {
                get { return new ChurchFalse(); }
            }

            public IChurchBoolean Just(T just)
            {
                return new ChurchTrue();
            }
        }

        // Functor
        public static IMaybe<TResult> Select<T, TResult>(
            this IMaybe<T> source,
            Func<T, TResult> selector)
        {
            return source.Match(
                new SelectMaybeParameters<T, TResult>(selector));
        }

        private class SelectMaybeParameters<T, TResult> :
            IMaybeParameters<T, IMaybe<TResult>>
        {
            private readonly Func<T, TResult> selector;

            public SelectMaybeParameters(Func<T, TResult> selector)
            {
                this.selector = selector;
            }

            public IMaybe<TResult> Nothing
            {
                get { return new Nothing<TResult>(); }
            }

            public IMaybe<TResult> Just(T just)
            {
                return new Just<TResult>(this.selector(just));
            }
        }

        // Monad
        public static IMaybe<T> Flatten<T>(this IMaybe<IMaybe<T>> source)
        {
            return source.Match(new FlattenMaybeParameters<T>());
        }

        private class FlattenMaybeParameters<T> :
            IMaybeParameters<IMaybe<T>, IMaybe<T>>
        {
            public IMaybe<T> Nothing
            {
                get { return new Nothing<T>(); }
            }

            public IMaybe<T> Just(IMaybe<T> just)
            {
                return just;
            }
        }

        public static IMaybe<TResult> SelectMany<T, TResult>(
            this IMaybe<T> source,
            Func<T, IMaybe<TResult>> selector)
        {
            return source.Select(selector).Flatten();
        }

        public static IMaybe<TResult> SelectMany<T, U, TResult>(
            this IMaybe<T> source,
            Func<T, IMaybe<U>> k,
            Func<T, U, TResult> s)
        {
            return source
                .SelectMany(x => k(x)
                    .SelectMany(y => new Just<TResult>(s(x, y))));
        }
    }
}
