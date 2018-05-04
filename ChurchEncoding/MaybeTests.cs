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
            var actual = sut.Match("empty", _ => "not empty");
            Assert.Equal("empty", actual);
        }

        [Fact]
        public void MatchFilled()
        {
            IMaybe<int> sut = new Just<int>(42);
            var actual = sut.Match("empty", i => i.ToString());
            Assert.Equal("42", actual);
        }

        [Fact]
        public void SimpleQueryExpressionWorks()
        {
            IMaybe<int> sut = new Just<int>(42);
            IMaybe<string> actual = from i in sut
                                    select i.ToString();
            Assert.Equal("42", actual.Match("nothing", x => x));
        }

        [Fact]
        public void MultiplyTwoNaturalNumbersUsingQueryExpression()
        {
            var mx = new Just<INaturalNumber>(NaturalNumber.Nine);
            var my = new Just<INaturalNumber>(NaturalNumber.Four);

            var actual = from x in mx
                         from y in my
                         select x.Multiply(y);

            Assert.Equal(36, actual.Match(NaturalNumber.Zero, x => x).Count());
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
            Assert.Equal(7, actual.Match(0, x => x));
        }

        [Fact]
        public void TryParseSqrtQueryExpressionExample()
        {
            var actual = from i in TryParse("49")
                         from d in Sqrt(i)
                         select d;
            Assert.Equal(7, actual.Match(0, x => x));
        }
    }
}
