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
    }
}
