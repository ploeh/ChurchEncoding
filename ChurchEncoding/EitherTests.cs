using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploeh.Samples.ChurchEncoding
{
    public class EitherTests
    {
        [Fact]
        public void MatchRight()
        {
            IEither<string, int> sut = new Right<string, int>(42);
            var actual = sut.Accept(new MatchVisitor());
            Assert.Equal("42", actual);
        }

        [Fact]
        public void MatchLeft()
        {
            IEither<string, int> sut = new Left<string, int>("foo");
            var actual = sut.Accept(new MatchVisitor());
            Assert.Equal("foo", actual);
        }

        private class MatchVisitor : IEitherVisitor<string, int, string>
        {
            public string VisitLeft(string left)
            {
                return left;
            }

            public string VisitRight(int right)
            {
                return right.ToString();
            }
        }

        public enum VoteError
        {
            Empty = 0,
            Tie
        }

        public static IEither<VoteError, T> FindWinner<T>(IReadOnlyCollection<T> votes)
        {
            var countedVotes = from v in votes
                               group v by v into g
                               let count = g.Count()
                               orderby count descending
                               select new { Vote = g.Key, Count = count };
            var c = countedVotes.Take(2).Count();

            if (c == 0)
                return new Left<VoteError, T>(VoteError.Empty);

            var x0 = countedVotes.ElementAt(0);
            if (c == 1)
                return new Right<VoteError, T>(x0.Vote);

            var x1 = countedVotes.ElementAt(1);
            if (Equals(x0.Count, x1.Count))
                return new Left<VoteError, T>(VoteError.Tie);

            return new Right<VoteError, T>(x0.Vote);
        }

        public static IEither<VoteError, T> FindWinner<T>(params T[] votes)
        {
            return FindWinner((IReadOnlyCollection<T>)votes);
        }

        [Theory]
        [InlineData(new string[0], "Empty")]
        [InlineData(new[] { "Foo" }, "Foo")]
        [InlineData(new[] { "Foo", "Bar" }, "Tie")]
        [InlineData(new[] { "Foo", "Bar", "Bar" }, "Bar")]
        [InlineData(new[] { "Foo", "Bar", "Foo" }, "Foo")]
        [InlineData(new[] { "Foo", "Bar", "Baz" }, "Tie")]
        [InlineData(new[] { "Foo", "Bar", "Bar", "Foo" }, "Tie")]
        [InlineData(new[] { "Foo", "Bar", "Baz", "Foo" }, "Foo")]
        public void FindWinnerReturnsCorrectResult(string[] votes, string expected)
        {
            var actual = FindWinner(votes);
            Assert.Equal(expected, actual.Accept(new FindWinnerVisitor()));
        }

        private class FindWinnerVisitor : IEitherVisitor<VoteError, string, string>
        {
            public string VisitLeft(VoteError left)
            {
                return left.ToString();
            }

            public string VisitRight(string right)
            {
                return right;
            }
        }

        [Fact]
        public void SelectLeft()
        {
            IEither<int, bool> sut = new Left<int, bool>(1337);
            var actual = sut.SelectLeft(i => i % 2 != 0);
            Assert.True(actual.Bifold());
        }

        [Fact]
        public void SelectRight()
        {
            IEither<bool, string> sut = new Right<bool, string>("foo");
            var actual = sut.SelectRight(s => s.StartsWith("f"));
            Assert.True(actual.Bifold());
        }

        private static T Id<T>(T x) => x;

        public static IEnumerable<object[]> BifunctorLawsData
        {
            get
            {
                yield return new[] { new  Left<string, int>("foo") };
                yield return new[] { new  Left<string, int>("bar") };
                yield return new[] { new  Left<string, int>("baz") };
                yield return new[] { new Right<string, int>(   42) };
                yield return new[] { new Right<string, int>( 1337) };
                yield return new[] { new Right<string, int>(    0) };
            }
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void SelectLeftObeysFirstFunctorLaw(IEither<string, int> e)
        {
            Assert.Equal(e, e.SelectLeft(Id));
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void SelectRightObeysFirstFunctorLaw(IEither<string, int> e)
        {
            Assert.Equal(e, e.SelectRight(Id));
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void SelectBothObeysIdentityLaw(IEither<string, int> e)
        {
            Assert.Equal(e, e.SelectBoth(Id, Id));
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void ConsistencyLawHolds(IEither<string, int> e)
        {
            bool f(string s) => string.IsNullOrWhiteSpace(s);
            DateTime g(int i) => new DateTime(i);

            Assert.Equal(e.SelectBoth(f, g), e.SelectRight(g).SelectLeft(f));
            Assert.Equal(
                e.SelectLeft(f).SelectRight(g),
                e.SelectRight(g).SelectLeft(f));
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void SecondFunctorLawHoldsForSelectLeft(IEither<string, int> e)
        {
            bool f(int x) => x % 2 == 0;
            int g(string s) => s.Length;

            Assert.Equal(
                e.SelectLeft(x => f(g(x))),
                e.SelectLeft(g).SelectLeft(f));
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void SecondFunctorLawHoldsForSelectRight(IEither<string, int> e)
        {
            char f(bool b) => b ? 'T' : 'F';
            bool g(int i) => i % 2 == 0;

            Assert.Equal(
                e.SelectRight(x => f(g(x))),
                e.SelectRight(g).SelectRight(f));
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void SelectBothCompositionLawHolds(IEither<string, int> e)
        {
            bool f(int x) => x % 2 == 0;
            int g(string s) => s.Length;
            char h(bool b) => b ? 'T' : 'F';
            bool i(int x) => x % 2 == 0;

            Assert.Equal(
                e.SelectBoth(x => f(g(x)), y => h(i(y))),
                e.SelectBoth(g, i).SelectBoth(f, h));
        }

        [Fact]
        public void UseSimpleQuerySyntax()
        {
            IEither<Guid, int> actual = from s in new Right<Guid, string>("foo")
                                        select s.Length;
            Assert.Equal(new Right<Guid, int>(3), actual);
        }

        [Fact]
        public void FindWinnerQuerySyntaxExample()
        {
            IEither<VoteError, bool> isVotePositive =
                from i in FindWinner(1, 2, -3, -1, 2, -1, -1)
                select i > 0;
            Assert.Equal(new Right<VoteError, bool>(false), isVotePositive);
        }

        [Fact]
        public void UseCartesianQuerySyntax()
        {
            var actual = from x in new Right<string, int>(42)
                         from y in new Left<string, double>("foo")
                         select Math.Pow(x, y);
            Assert.Equal("foo", actual.Select(_ => "bar").Bifold());
        }

        public static IEither<string, DateTime> TryParseDate(string candidate)
        {
            if (DateTime.TryParse(candidate, out var d))
                return new Right<string, DateTime>(d);
            else
                return new Left<string, DateTime>(candidate);
        }

        public static IEither<string, TimeSpan> TryParseDuration(string candidate)
        {
            if (TimeSpan.TryParse(candidate, out var ts))
                return new Right<string, TimeSpan>(ts);
            else
                return new Left<string, TimeSpan>(candidate);
        }

        [Fact]
        public void NestedParsingExample()
        {
            IEither<string, DateTime> dt = TryParseDate("2022-03-21");
            IEither<string, TimeSpan> ts = TryParseDuration("2");

            IEither<string, DateTime> result =
                from d in dt
                from dur in ts
                select d + dur;

            Assert.Equal(
                new Right<string, DateTime>(new DateTime(2022, 3, 23)),
                result);
        }

        [Theory]
        [InlineData("2")]
        [InlineData("2.3:00")]
        [InlineData("4.5:30")]
        [InlineData("0:33:44")]
        [InlineData("foo")]
        public void LeftIdentityLaw(string a)
        {
            Func<string, IEither<string, string>> @return = s => new Right<string, string>(s);
            Func<string, IEither<string, TimeSpan>> h = TryParseDuration;

            Assert.Equal(@return(a).SelectMany(h), h(a));
        }

        [Theory]
        [InlineData("2022-03-22")]
        [InlineData("2022-03-21T16:57")]
        [InlineData("bar")]
        public void RightIdentityLaw(string a)
        {
            Func<string, IEither<string, DateTime>> f = TryParseDate;
            Func<DateTime, IEither<string, DateTime>> @return = d => new Right<string, DateTime>(d);

            IEither<string, DateTime> m = f(a);

            Assert.Equal(m.SelectMany(@return), m);
        }

        public static IEither<string, double> DaysForward(TimeSpan ts)
        {
            if (ts < TimeSpan.Zero)
                return new Left<string, double>($"Negative durations not allowed: {ts}.");

            return new Right<string, double>(ts.TotalDays);
        }

        public static IEither<string, int> Nat(double d)
        {
            if (d % 1 != 0)
                return new Left<string, int>($"Non-integers not allowed: {d}.");
            if (d < 1)
                return new Left<string, int>($"Non-positive numbers not allowed: {d}.");

            return new Right<string, int>((int)d);
        }

        [Theory]
        [InlineData("2")]
        [InlineData("-2.3:00")]
        [InlineData("4.5:30")]
        [InlineData("0:33:44")]
        [InlineData("0")]
        [InlineData("foo")]
        public void AssociativityLaw(string a)
        {
            Func<string, IEither<string, TimeSpan>> f = TryParseDuration;
            Func<TimeSpan, IEither<string, double>> g = DaysForward;
            Func<double, IEither<string, int>> h = Nat;

            var m = f(a);

            Assert.Equal(m.SelectMany(g).SelectMany(h), m.SelectMany(x => g(x).SelectMany(h)));
        }
    }
}
