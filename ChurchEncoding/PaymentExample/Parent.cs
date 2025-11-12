using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding.PaymentExample
{
    public class Parent : IPaymentType
    {
        private readonly PaymentService paymentService;

        public Parent(PaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        public T Accept<T>(IPaymentTypeVisitor<T> visitor)
        {
            return visitor.VisitParent(paymentService);
        }

        public override bool Equals(object obj)
        {
            return obj is Parent parent &&
                   EqualityComparer<PaymentService>.Default.Equals(paymentService, parent.paymentService);
        }

        public override int GetHashCode()
        {
            return 1075050752 + EqualityComparer<PaymentService>.Default.GetHashCode(paymentService);
        }
    }
}
