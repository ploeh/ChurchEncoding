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

        private class ToCanonicalPeriodVisitor : IPeriodVisitor<IEither<int, IEither<(int, int), (int, (int, int))>>>
        {
            public IEither<int, IEither<(int, int), (int, (int, int))>> VisitDay(int year, int month, int day)
            {
                throw new System.NotImplementedException();
            }

            public IEither<int, IEither<(int, int), (int, (int, int))>> VisitMonth(int year, int month)
            {
                throw new System.NotImplementedException();
            }

            public IEither<int, IEither<(int, int), (int, (int, int))>> VisitYear(int year)
            {
                return new Left<int, IEither<(int, int), (int, (int, int))>>(year);
            }
        }
    }
}
