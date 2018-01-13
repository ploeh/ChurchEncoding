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
            return m.Accept(new IsNothingMaybeVisitor<T>());
        }

        private class IsNothingMaybeVisitor<T> :
            IMaybeVisitor<T, IChurchBoolean>
        {
            public IChurchBoolean VisitNothing
            {
                get { return new ChurchTrue(); }
            }

            public IChurchBoolean VisitJust(T just)
            {
                return new ChurchFalse();
            }
        }

        public static IChurchBoolean IsJust<T>(this IMaybe<T> m)
        {
            return m.Accept(new IsJustMaybeVisitor<T>());
        }

        private class IsJustMaybeVisitor<T> : IMaybeVisitor<T, IChurchBoolean>
        {
            public IChurchBoolean VisitNothing
            {
                get { return new ChurchFalse(); }
            }

            public IChurchBoolean VisitJust(T just)
            {
                return new ChurchTrue();
            }
        }

        // Functor
        public static IMaybe<TResult> Select<T, TResult>(
            this IMaybe<T> source,
            Func<T, TResult> selector)
        {
            return source.Accept(new SelectMaybeVisitor<T, TResult>(selector));
        }

        private class SelectMaybeVisitor<T, TResult> :
            IMaybeVisitor<T, IMaybe<TResult>>
        {
            private readonly Func<T, TResult> selector;

            public SelectMaybeVisitor(Func<T, TResult> selector)
            {
                this.selector = selector;
            }

            public IMaybe<TResult> VisitNothing
            {
                get { return new Nothing<TResult>(); }
            }

            public IMaybe<TResult> VisitJust(T just)
            {
                return new Just<TResult>(this.selector(just));
            }
        }

        // Monad
        public static IMaybe<T> Flatten<T>(this IMaybe<IMaybe<T>> source)
        {
            return source.Accept(new FlattenMaybeVisitor<T>());
        }

        private class FlattenMaybeVisitor<T> :
            IMaybeVisitor<IMaybe<T>, IMaybe<T>>
        {
            public IMaybe<T> VisitNothing
            {
                get { return new Nothing<T>(); }
            }

            public IMaybe<T> VisitJust(IMaybe<T> just)
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
