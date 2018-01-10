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
            return n.Match(
                0,
                p => 1 + p.Count());
        }

        public static INaturalNumber Add(
            this INaturalNumber x,
            INaturalNumber y)
        {
            return x.Match(
                y,
                p => new Successor(p.Add(y)));
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
            return x.Match(
                new Zero(),
                px => y.Match(
                    new Zero(),
                    py =>
                        One
                        .Add(px)
                        .Add(py)
                        .Add(px.Multiply(py))));
        }

        public static IChurchBoolean IsZero(this INaturalNumber n)
        {
            return n.Match<IChurchBoolean>(
                new ChurchTrue(),
                _ => new ChurchFalse());
        }

        public static IChurchBoolean IsEven(this INaturalNumber n)
        {
            return n.Match(
                new ChurchTrue(),        // 0 is even, so true
                p1 => p1.Match(          // Match previous
                    new ChurchFalse(),   // If 0 then successor was 1
                    p2 => p2.IsEven())); // Eval previous' previous
        }

        public static IChurchBoolean IsOdd(this INaturalNumber n)
        {
            return new ChurchNot(n.IsEven());
        }
    }
}
