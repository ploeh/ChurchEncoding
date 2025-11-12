using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding.PaymentExample
{
    public class Child : IPaymentType
    {
        private readonly ChildPaymentService childPaymentService;

        public Child(ChildPaymentService childPaymentService)
        {
            this.childPaymentService = childPaymentService;
        }

        public T Accept<T>(IPaymentTypeVisitor<T> visitor)
        {
            return visitor.VisitChild(childPaymentService);
        }

        public override bool Equals(object obj)
        {
            return obj is Child child &&
                   EqualityComparer<ChildPaymentService>.Default.Equals(childPaymentService, child.childPaymentService);
        }

        public override int GetHashCode()
        {
            return 499700248 + EqualityComparer<ChildPaymentService>.Default.GetHashCode(childPaymentService);
        }
    }
}
