using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploeh.Samples.ChurchEncoding.PaymentExample
{
    public class PaymentTypeTests
    {
        [Fact]
        public void IndividualToJsonReturnsCorrectResult()
        {
            var sut = new Individual(new PaymentService("MasterCard", "Pay"));

            var actual = sut.ToJson();

            Assert.Equal("MasterCard", actual.Name);
            Assert.Equal("Pay", actual.Action);
            Assert.False(actual.StartRecurrent.ToBool());
            Assert.True(actual.TransactionKey.IsNothing().ToBool());
        }

        [Fact]
        public void ParentToJsonReturnsCorrectResult()
        {
            var sut = new Parent(new PaymentService("MasterCard", "Pay"));

            var actual = sut.ToJson();

            Assert.Equal("MasterCard", actual.Name);
            Assert.Equal("Pay", actual.Action);
            Assert.True(actual.StartRecurrent.ToBool());
            Assert.True(actual.TransactionKey.IsNothing().ToBool());
        }

        [Fact]
        public void ChildToJsonReturnsCorrectResult()
        {
            var sut =
                new Child(
                    new ChildPaymentService(
                        "12345",
                        new PaymentService("MasterCard", "Pay")));

            var actual = sut.ToJson();

            Assert.Equal("MasterCard", actual.Name);
            Assert.Equal("Pay", actual.Action);
            Assert.False(actual.StartRecurrent.ToBool());
            Assert.Equal(
                "12345", 
                actual.TransactionKey.Match(
                    new MaybeParameters<string, string>(
                        nothing :   "",
                        just : x => x)));
        }
    }
}
