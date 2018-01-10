using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploeh.Samples.ChurchEncoding
{
    public class NatualNumberTests
    {
        [Fact]
        public void CountZero()
        {
            var zero = new Zero();
            var actual = zero.Count();
            Assert.Equal(0, actual);
        }

        [Fact]
        public void CountOne()
        {
            var one = new Successor(new Zero());
            var actual = one.Count();
            Assert.Equal(1, actual);
        }

        [Fact]
        public void CountTwo()
        {
            var two = new Successor(new Successor(new Zero()));
            var actual = two.Count();
            Assert.Equal(2, actual);
        }

        [Fact]
        public void CountThree()
        {
            var three =
                new Successor(new Successor(new Successor(new Zero())));
            var actual = three.Count();
            Assert.Equal(3, actual);
        }

        [Fact]
        public void TwoPlusThreeIsFive()
        {
            var two = new Successor(new Successor(new Zero()));
            var three =
                new Successor(new Successor(new Successor(new Zero())));

            var actual = two.Add(three);

            Assert.Equal(5, actual.Count());
        }

        [Fact]
        public void OnePlusTwoPlusThreeIsSix()
        {
            var one = new Successor(new Zero());
            var two = new Successor(new Successor(new Zero()));
            var three =
                new Successor(new Successor(new Successor(new Zero())));

            var actual = one.Add(two).Add(three);

            Assert.Equal(6, actual.Count());
        }

        [Fact]
        public void ZeroIsEven()
        {
            var zero = new Zero();
            var actual = zero.IsEven();
            Assert.True(actual.ToBool());
        }

        [Fact]
        public void OneIsNotEven()
        {
            var one = new Successor(new Zero());
            var actual = one.IsEven();
            Assert.False(actual.ToBool());
        }

        [Fact]
        public void TwoIsEven()
        {
            var two = new Successor(new Successor(new Zero()));
            var actual = two.IsEven();
            Assert.True(actual.ToBool());
        }

        [Fact]
        public void ThreeIsNotEven()
        {
            var three =
                new Successor(new Successor(new Successor(new Zero())));
            var actual = three.IsEven();
            Assert.False(actual.ToBool());
        }

        [Fact]
        public void SixIsNotOdd()
        {
            var six =
                new Successor(                            // 6
                    new Successor(                        // 5
                        new Successor(                    // 4
                            new Successor(                // 3
                                new Successor(            // 2
                                    new Successor(        // 1
                                        new Zero())))))); // 0
            var actual = six.IsOdd();
            Assert.False(actual.ToBool());
        }

        [Fact]
        public void SevenIsOdd()
        {
            var seven =
                new Successor(                                 // 7
                    new Successor(                             // 6
                        new Successor(                         // 5
                            new Successor(                     // 4
                                new Successor(                 // 3
                                    new Successor(             // 2
                                        new Successor(         // 1
                                            new Zero()))))))); // 0
            var actual = seven.IsOdd();
            Assert.True(actual.ToBool());
        }

        [Fact]
        public void SixTimesSevenIsFortyTwo()
        {
            var actual = NaturalNumber.Six.Multiply(NaturalNumber.Seven);
            Assert.Equal(42, actual.Count());
        }
    }
}
