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
            var actual = sut.Match(s => s, i => i.ToString());
            Assert.Equal("42", actual);
        }

        [Fact]
        public void MatchLeft()
        {
            IEither<string, int> sut = new Left<string, int>("foo");
            var actual = sut.Match(s => s, i => i.ToString());
            Assert.Equal("foo", actual);
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
            Assert.Equal(expected, actual.Match(ve => ve.ToString(), s => s));
        }
    }
}
