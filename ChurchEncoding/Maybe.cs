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
            return m.Match(
                new MaybeParameters<T, IChurchBoolean>(
                    nothing :   new ChurchTrue(), 
                    just : _ => new ChurchFalse()));
        }

        public static IChurchBoolean IsJust<T>(this IMaybe<T> m)
        {
            return m.Match(
                new MaybeParameters<T, IChurchBoolean>(
                    nothing :   new ChurchFalse(),
                    just : _ => new ChurchTrue()));
        }

        // Functor
        public static IMaybe<TResult> Select<T, TResult>(
            this IMaybe<T> source,
            Func<T, TResult> selector)
        {
            return source.Match(
                new MaybeParameters<T, IMaybe<TResult>>(
                    nothing :   new Nothing<TResult>(),
                    just : x => new Just<TResult>(selector(x))));
        }

        // Monad
        public static IMaybe<T> Flatten<T>(this IMaybe<IMaybe<T>> source)
        {
            return source.Match(
                new MaybeParameters<IMaybe<T>, IMaybe<T>>(
                    nothing :   new Nothing<T>(),
                    just : x => x));
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
