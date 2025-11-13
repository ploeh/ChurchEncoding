using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
                actual.TransactionKey.Accept(
                    new FromMaybeVisitor<string>("")));
        }

        [Fact]
        public void IndividualToCanonical()
        {
            var sut = new Individual(new PaymentService("MasterCard", "Pay"));
            var actual = sut.Accept(new ToCanonicalPaymentTypeVisitor());
            Assert.Equal(
                new Left<(string, string), IEither<(string, string), (string, (string, string))>>(
                    ("MasterCard", "Pay")),
                actual);
        }

        [Fact]
        public void ParentToCanonical()
        {
            var sut = new Parent(new PaymentService("MasterCard", "Pay"));
            var actual = sut.Accept(new ToCanonicalPaymentTypeVisitor());
            Assert.Equal(
                new Right<(string, string), IEither<(string, string), (string, (string, string))>>(
                    new Left<(string, string), (string, (string, string))>(
                        ("MasterCard", "Pay"))),
                actual);
        }

        [Fact]
        public void ChildToCanonical()
        {
            var sut =
                new Child(
                    new ChildPaymentService(
                        "12345",
                        new PaymentService("MasterCard", "Pay")));
            var actual = sut.Accept(new ToCanonicalPaymentTypeVisitor());
            Assert.Equal(
                new Right<(string, string), IEither<(string, string), (string, (string, string))>>(
                    new Right<(string, string), (string, (string, string))>(
                        ("12345", ("MasterCard", "Pay")))),
                actual);
        }

        private class ToCanonicalPaymentTypeVisitor :
            IPaymentTypeVisitor<
                IEither<(string, string),
                IEither<(string, string), (string, (string, string))>>>
        {
            public IEither<(string, string), IEither<(string, string), (string, (string, string))>>
                VisitChild(ChildPaymentService child)
            {
                return new Right<(string, string), IEither<(string, string), (string, (string, string))>>(
                    new Right<(string, string), (string, (string, string))>(
                        (
                            child.OriginalTransactionKey,
                            (
                                child.PaymentService.Name,
                                child.PaymentService.Action
                            )
                        )));
            }

            public IEither<(string, string), IEither<(string, string), (string, (string, string))>>
                VisitIndividual(PaymentService individual)
            {
                return new Left<(string, string), IEither<(string, string), (string, (string, string))>>(
                    (individual.Name, individual.Action));
            }

            public IEither<(string, string), IEither<(string, string), (string, (string, string))>>
                VisitParent(PaymentService parent)
            {
                return new Right<(string, string), IEither<(string, string), (string, (string, string))>>(
                    new Left<(string, string), (string, (string, string))>(
                        (parent.Name, parent.Action)));
            }
        }

        [Fact]
        public void CanonicalToIndividual()
        {
            var sut = new Left<(string, string), IEither<(string, string), (string, (string, string))>>(
                ("MasterCard", "Pay"));
            var actual = sut.Accept(new FromCanonicalPaymentTypeVisitor());
            Assert.Equal(
                new Individual(new PaymentService("MasterCard", "Pay")),
                actual);
        }

        [Fact]
        public void CanonicalToParent()
        {
            var sut = new Right<(string, string), IEither<(string, string), (string, (string, string))>>(
                new Left<(string, string), (string, (string, string))>(
                    ("MasterCard", "Pay")));
            var actual = sut.Accept(new FromCanonicalPaymentTypeVisitor());
            Assert.Equal(
                new Parent(new PaymentService("MasterCard", "Pay")),
                actual);
        }

        [Fact]
        public void CanonicalToChild()
        {
            var sut = new Right<(string, string), IEither<(string, string), (string, (string, string))>>(
                new Right<(string, string), (string, (string, string))>(
                    ("12345", ("MasterCard", "Pay"))));
            var actual = sut.Accept(new FromCanonicalPaymentTypeVisitor());
            Assert.Equal(
                new Child(
                    new ChildPaymentService(
                        "12345",
                        new PaymentService("MasterCard", "Pay"))),
                actual);
        }

        private class FromCanonicalPaymentTypeVisitor :
            IEitherVisitor<
                (string, string),
                IEither<(string, string), (string, (string, string))>, IPaymentType>
        {
            public IPaymentType VisitLeft((string, string) left)
            {
                var (name, action) = left;
                return new Individual(new PaymentService(name, action));
            }

            public IPaymentType VisitRight(IEither<(string, string), (string, (string, string))> right)
            {
                return right.Accept(new CanonicalParentChildPaymentTypeVisitor());
            }

            private class CanonicalParentChildPaymentTypeVisitor :
                IEitherVisitor<(string, string), (string, (string, string)), IPaymentType>
            {
                public IPaymentType VisitLeft((string, string) left)
                {
                    var (name, action) = left;
                    return new Parent(new PaymentService(name, action));
                }

                public IPaymentType VisitRight((string, (string, string)) right)
                {
                    var (k, (n, a)) = right;
                    return new Child(new ChildPaymentService(k, new PaymentService(n, a)));
                }
            }
        }

        private sealed class PaymentTypes : TheoryData<IPaymentType>
        {
            public PaymentTypes()
            {
                Add(new Individual(new PaymentService("Visa", "Pay")));
                Add(new Parent(new PaymentService("AmEx", "Pay")));
                Add(
                    new Child(
                        new ChildPaymentService(
                            "1234ABCD",
                            new PaymentService("DisCard", "Pay"))));
            }
        }

        [Theory, ClassData(typeof(PaymentTypes))]
        public void IsomorphismViaEither(IPaymentType expected)
        {
            var actual = expected
                .Accept(new ToCanonicalPaymentTypeVisitor())
                .Accept(new FromCanonicalPaymentTypeVisitor());
            Assert.Equal(expected, actual);
        }
    }
}
