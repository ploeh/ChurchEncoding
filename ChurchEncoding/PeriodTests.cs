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

        private class ToCanonicalPeriodVisitor : IPeriodVisitor<IEither<int, IEither<(int, int), (int, (int, int))>>>
        {
            public IEither<int, IEither<(int, int), (int, (int, int))>> VisitDay(int year, int month, int day)
            {
                throw new System.NotImplementedException();
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
    }
}
