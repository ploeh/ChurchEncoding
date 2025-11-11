using Xunit;

namespace Ploeh.Samples.ChurchEncoding
{
    public sealed class PeriodTests
    {
        [Theory]
        [InlineData(2024)]
        [InlineData(1999)]
        [InlineData(0)]
        public void YearToCanonical(int y)
        {
            var sut = new Year(y);
            var actual = sut.Accept(new ToCanonicalPeriodVisitor());
            Assert.Equal(
                new Left<int, IEither<(int, int), (int, (int, int))>>(y),
                actual);
        }

        [Theory]
        [InlineData(2024, 5)]
        [InlineData(1999, 12)]
        [InlineData(0, 1)]
        public void MonthToCanonical(int y, int m)
        {
            var sut = new Month(y, m);
            var actual = sut.Accept(new ToCanonicalPeriodVisitor());
            Assert.Equal(
                new Right<int, IEither<(int, int), (int, (int, int))>>(
                    new Left<(int, int), (int, (int, int))>((y, m))),
                actual);
        }

        [Theory]
        [InlineData(2024, 11, 11)]
        [InlineData(1993, 1, 1)]
        [InlineData(1774, 9, 23)]
        public void DayToCanonical(int y, int m, int d)
        {
            var sut = new Day(y, m, d);
            var actual = sut.Accept(new ToCanonicalPeriodVisitor());
            Assert.Equal(
                new Right<int, IEither<(int, int), (int, (int, int))>>(
                    new Right<(int, int), (int, (int, int))>((y, (m, d)))),
                actual);
        }

        private class ToCanonicalPeriodVisitor :
            IPeriodVisitor<IEither<int, IEither<(int, int), (int, (int, int))>>>
        {
            public IEither<int, IEither<(int, int), (int, (int, int))>> VisitDay(
                int year,
                int month,
                int day)
            {
                return new Right<int, IEither<(int, int), (int, (int, int))>>(
                    new Right<(int, int), (int, (int, int))>((year, (month, day))));
            }

            public IEither<int, IEither<(int, int), (int, (int, int))>> VisitMonth(
                int year,
                int month)
            {
                return new Right<int, IEither<(int, int), (int, (int, int))>>(
                    new Left<(int, int), (int, (int, int))>((year, month)));
            }

            public IEither<int, IEither<(int, int), (int, (int, int))>> VisitYear(int year)
            {
                return new Left<int, IEither<(int, int), (int, (int, int))>>(year);
            }
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(1970)]
        [InlineData(1491)]
        public void CanonicalToYear(int y)
        {
            var sut = new Left<int, IEither<(int, int), (int, (int, int))>>(y);
            var actual = sut.Accept(new ToPeriodVisitor());
            Assert.Equal(new Year(y), actual);
        }

        [Theory]
        [InlineData(2013, 12)]
        [InlineData(2002, 1)]
        [InlineData(1969, 9)]
        public void CanonicalToMonth(int y, int m)
        {
            var sut = new Right<int, IEither<(int, int), (int, (int, int))>>(
                new Left<(int, int), (int, (int, int))>((y, m)));
            var actual = sut.Accept(new ToPeriodVisitor());
            Assert.Equal(new Month(y, m), actual);
        }

        [Theory]
        [InlineData(2024, 6, 15)]
        [InlineData(2010, 2, 30)]
        [InlineData(1452, 10, 20)]
        public void CanonicalToDay(int y, int m, int d)
        {
            var sut = new Right<int, IEither<(int, int), (int, (int, int))>>(
                new Right<(int, int), (int, (int, int))>((y, (m, d))));
            var actual = sut.Accept(new ToPeriodVisitor());
            Assert.Equal(new Day(y, m, d), actual);
        }

        private class ToPeriodVisitor :
            IEitherVisitor<int, IEither<(int, int), (int, (int, int))>, IPeriod>
        {
            public IPeriod VisitLeft(int left)
            {
                return new Year(left);
            }

            public IPeriod VisitRight(IEither<(int, int), (int, (int, int))> right)
            {
                return right.Accept(new ToMonthOrDayVisitor());
            }

            private class ToMonthOrDayVisitor :
                IEitherVisitor<(int, int), (int, (int, int)), IPeriod>
            {
                public IPeriod VisitLeft((int, int) left)
                {
                    var (y, m) = left;
                    return new Month(y, m);
                }

                public IPeriod VisitRight((int, (int, int)) right)
                {
                    var (y, (m, d)) = right;
                    return new Day(y, m, d);
                }
            }
        }

        private class PeriodData : TheoryData<IPeriod>
        {
            public PeriodData()
            {
                Add(new Year(2024));
                Add(new Year(1999));
                Add(new Month(2024, 6));
                Add(new Month(1997, 12));
                Add(new Day(2024, 6, 15));
                Add(new Day(1776, 7, 4));
            }
        }

        [Theory, ClassData(typeof(PeriodData))]
        public void IsomorphismViaEither(IPeriod expected)
        {
            var actual = expected
                .Accept(new ToCanonicalPeriodVisitor())
                .Accept(new ToPeriodVisitor());
            Assert.Equal(expected, actual);
        }
    }
}
