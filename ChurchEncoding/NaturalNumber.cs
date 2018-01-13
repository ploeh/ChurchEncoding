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
            return n.Match(new CountNaturalNumberParameters());
        }

        private class CountNaturalNumberParameters : 
            INaturalNumberParameters<int>
        {
            public int Zero
            {
                get { return 0; }
            }

            public int Succ(INaturalNumber predecessor)
            {
                return 1 + predecessor.Count();
            }
        }

        public static INaturalNumber Add(
            this INaturalNumber x,
            INaturalNumber y)
        {
            return x.Match(new AddNaturalNumberParameters(y));
        }

        private class AddNaturalNumberParameters :
            INaturalNumberParameters<INaturalNumber>
        {
            private readonly INaturalNumber other;

            public AddNaturalNumberParameters(INaturalNumber other)
            {
                this.other = other;
            }

            public INaturalNumber Zero
            {
                get { return other; }
            }

            public INaturalNumber Succ(INaturalNumber predecessor)
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
            return x.Match(new MultiplyNaturalNumberParameters(y));
        }

        private class MultiplyNaturalNumberParameters : 
            INaturalNumberParameters<INaturalNumber>
        {
            private readonly INaturalNumber other;

            public MultiplyNaturalNumberParameters(INaturalNumber other)
            {
                this.other = other;
            }

            public INaturalNumber Zero
            {
                get { return new Zero(); }
            }

            public INaturalNumber Succ(INaturalNumber predecessor)
            {
                return this.other.Match(
                    new MultiplyOtherNaturalNumberParameters(predecessor));
            }

            private class MultiplyOtherNaturalNumberParameters :
                INaturalNumberParameters<INaturalNumber>
            {
                private readonly INaturalNumber other;

                public MultiplyOtherNaturalNumberParameters(INaturalNumber other)
                {
                    this.other = other;
                }

                public INaturalNumber Zero
                {
                    get { return new Zero(); }
                }

                public INaturalNumber Succ(INaturalNumber predecessor)
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
            return n.Match(new IsZeroNaturalNumberParameters());
        }

        private class IsZeroNaturalNumberParameters :
            INaturalNumberParameters<IChurchBoolean>
        {
            public IChurchBoolean Zero
            {
                get { return new ChurchTrue(); }
            }

            public IChurchBoolean Succ(INaturalNumber predecessor)
            {
                return new ChurchFalse();
            }
        }

        public static IChurchBoolean IsEven(this INaturalNumber n)
        {
            return n.Match(new IsEvenNaturalNumberParameters());
        }

        private class IsEvenNaturalNumberParameters :
            INaturalNumberParameters<IChurchBoolean>
        {
            // 0 is even, so true
            public IChurchBoolean Zero
            {
                get { return new ChurchTrue(); }
            }

            public IChurchBoolean Succ(INaturalNumber predecessor)
            {
                // Match previous
                return predecessor.Match(
                    new IsEvenPredecessorNaturalNumberParameters());
            }

            private class IsEvenPredecessorNaturalNumberParameters :
                INaturalNumberParameters<IChurchBoolean>
            {
                // If 0, the successor was 1
                public IChurchBoolean Zero
                {
                    get { return new ChurchFalse(); }
                }

                public IChurchBoolean Succ(INaturalNumber predecessor)
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
