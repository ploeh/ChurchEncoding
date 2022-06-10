using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploeh.Samples.ChurchEncoding
{
    public class MaybeTests
    {
        [Fact]
        public void MatchEmpty()
        {
            IMaybe<Guid> sut = new Nothing<Guid>();
            var actual = sut.Accept(new MatchEmptyMaybeVisitor<Guid>());
            Assert.Equal("empty", actual);
        }

        private class MatchEmptyMaybeVisitor<T> :
            IMaybeVisitor<T, string>
        {
            public string VisitNothing
            {
                get { return "empty"; }
            }

            public string VisitJust(T just)
            {
                return "not empty";
            }
        }

        [Fact]
        public void MatchFilled()
        {
            IMaybe<int> sut = new Just<int>(42);
            var actual = sut.Accept(new MatchFilledMaybeVisitor<int>());
            Assert.Equal("42", actual);
        }

        private class MatchFilledMaybeVisitor<T> :
            IMaybeVisitor<T, string>
        {
            public string VisitNothing
            {
                get { return "empty"; }
            }

            public string VisitJust(T just)
            {
                return just.ToString();
            }
        }

        [Fact]
        public void SimpleQueryExpressionWorks()
        {
            IMaybe<int> sut = new Just<int>(42);
            IMaybe<string> actual = from i in sut
                                    select i.ToString();
            Assert.Equal(
                "42",
                actual.Accept(new FromMaybeVisitor<string>("nothing")));
        }

        [Fact]
        public void MultiplyTwoNaturalNumbersUsingQueryExpression()
        {
            var mx = new Just<INaturalNumber>(NaturalNumber.Nine);
            var my = new Just<INaturalNumber>(NaturalNumber.Four);

            var actual = from x in mx
                         from y in my
                         select x.Multiply(y);

            Assert.Equal(
                36,
                actual
                    .Accept(
                        new FromMaybeVisitor<INaturalNumber>(
                            NaturalNumber.Zero))
                    .Count());
        }

        public static IMaybe<int> TryParse(string s)
        {
            int i;
            if (int.TryParse(s, out i))
                return new Just<int>(i);

            return new Nothing<int>();
        }

        public static IMaybe<double> Sqrt(double d)
        {
            var result = Math.Sqrt(d);
            switch (result)
            {
                case double.NaN:
                    return new Nothing<double>();
                case double.PositiveInfinity:
                    return new Nothing<double>();
                default:
                    return new Just<double>(result);
            }
        }

        [Fact]
        public void TryParseSqrtFlattenExample()
        {
            var actual = TryParse("49").Select(i => Sqrt(i)).Flatten();
            Assert.Equal(7, actual.Accept(new FromMaybeVisitor<double>(0)));
        }

        [Fact]
        public void TryParseSqrtQueryExpressionExample()
        {
            var actual = from i in TryParse("49")
                         from d in Sqrt(i)
                         select d;
            Assert.Equal(7, actual.Accept(new FromMaybeVisitor<double>(0)));
        }

        [Fact]
        public void NothingToList()
        {
            IMaybe<double> maybe = new Nothing<double>();
            IEnumerable<double> actual = maybe.ToList();
            Assert.Empty(actual);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData( 0)]
        [InlineData(15)]
        public void JustToList(double d)
        {
            IMaybe<double> maybe = new Just<double>(d);
            IEnumerable<double> actual = maybe.ToList();
            Assert.Single(actual, d);
        }
    }
}
