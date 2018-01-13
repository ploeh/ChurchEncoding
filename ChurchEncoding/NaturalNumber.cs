using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding
{
    public static class NaturalNumber
    {
        public static INaturalNumber  Zero = new Zero();
        public static INaturalNumber   One = new Successor(Zero);
        public static INaturalNumber   Two = new Successor(One);
        public static INaturalNumber Three = new Successor(Two);
        public static INaturalNumber  Four = new Successor(Three);
        public static INaturalNumber  Five = new Successor(Four);
        public static INaturalNumber   Six = new Successor(Five);
        public static INaturalNumber Seven = new Successor(Six);
        public static INaturalNumber Eight = new Successor(Seven);
        public static INaturalNumber  Nine = new Successor(Eight);

        // More memmbers go here...

        public static int Count(this INaturalNumber n)
        {
            return n.Accept(new CountNaturalNumberVisitor());
        }

        private class CountNaturalNumberVisitor : 
            INaturalNumberVisitor<int>
        {
            public int VisitZero
            {
                get { return 0; }
            }

            public int VisitSucc(INaturalNumber predecessor)
            {
                return 1 + predecessor.Count();
            }
        }

        public static INaturalNumber Add(
            this INaturalNumber x,
            INaturalNumber y)
        {
            return x.Accept(new AddNaturalNumberVisitor(y));
        }

        private class AddNaturalNumberVisitor :
            INaturalNumberVisitor<INaturalNumber>
        {
            private readonly INaturalNumber other;

            public AddNaturalNumberVisitor(INaturalNumber other)
            {
                this.other = other;
            }

            public INaturalNumber VisitZero
            {
                get { return other; }
            }

            public INaturalNumber VisitSucc(INaturalNumber predecessor)
            {
                return new Successor(predecessor.Add(other));
            }
        }

        // The formula used here is
        // x * y = 1 + (x - 1) + (y - 1) + ((x - 1) * (y - 1))
        // It follows like this:
        // x* y =
        // (x - 1 + 1) * (y - 1 + 1) =
        // ((x - 1) + 1) * ((y - 1) + 1) =
        // ((x - 1) * (y - 1)) + ((x - 1) * 1) + ((y - 1) * 1) + 1 * 1 =
        // ((x - 1) * (y - 1)) + (x - 1) + (y - 1) + 1
        public static INaturalNumber Multiply(
            this INaturalNumber x,
            INaturalNumber y)
        {
            return x.Accept(new MultiplyNaturalNumberVisitor(y));
        }

        private class MultiplyNaturalNumberVisitor : 
            INaturalNumberVisitor<INaturalNumber>
        {
            private readonly INaturalNumber other;

            public MultiplyNaturalNumberVisitor(INaturalNumber other)
            {
                this.other = other;
            }

            public INaturalNumber VisitZero
            {
                get { return new Zero(); }
            }

            public INaturalNumber VisitSucc(INaturalNumber predecessor)
            {
                return this.other.Accept(
                    new MultiplyOtherNaturalNumberVisitor(predecessor));
            }

            private class MultiplyOtherNaturalNumberVisitor :
                INaturalNumberVisitor<INaturalNumber>
            {
                private readonly INaturalNumber other;

                public MultiplyOtherNaturalNumberVisitor(INaturalNumber other)
                {
                    this.other = other;
                }

                public INaturalNumber VisitZero
                {
                    get { return new Zero(); }
                }

                public INaturalNumber VisitSucc(INaturalNumber predecessor)
                {
                    return
                        One
                        .Add(other)
                        .Add(predecessor)
                        .Add(other.Multiply(predecessor));
                }
            }
        }

        public static IChurchBoolean IsZero(this INaturalNumber n)
        {
            return n.Accept(new IsZeroNaturalNumberVisitor());
        }

        private class IsZeroNaturalNumberVisitor :
            INaturalNumberVisitor<IChurchBoolean>
        {
            public IChurchBoolean VisitZero
            {
                get { return new ChurchTrue(); }
            }

            public IChurchBoolean VisitSucc(INaturalNumber predecessor)
            {
                return new ChurchFalse();
            }
        }

        public static IChurchBoolean IsEven(this INaturalNumber n)
        {
            return n.Accept(new IsEvenNaturalNumberVisitor());
        }

        private class IsEvenNaturalNumberVisitor :
            INaturalNumberVisitor<IChurchBoolean>
        {
            // 0 is even, so true
            public IChurchBoolean VisitZero
            {
                get { return new ChurchTrue(); }
            }

            public IChurchBoolean VisitSucc(INaturalNumber predecessor)
            {
                // Match previous
                return predecessor.Accept(
                    new IsEvenPredecessorNaturalNumberVisitor());
            }

            private class IsEvenPredecessorNaturalNumberVisitor :
                INaturalNumberVisitor<IChurchBoolean>
            {
                // If 0, the successor was 1
                public IChurchBoolean VisitZero
                {
                    get { return new ChurchFalse(); }
                }

                public IChurchBoolean VisitSucc(INaturalNumber predecessor)
                {
                    // Evaluate previous' previous value
                    return predecessor.IsEven();
                }
            }
        }

        public static IChurchBoolean IsOdd(this INaturalNumber n)
        {
            return new ChurchNot(n.IsEven());
        }
    }
}
