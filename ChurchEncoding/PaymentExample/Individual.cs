using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding.PaymentExample
{
    public class Individual : IPaymentType
    {
        private readonly PaymentService paymentService;

        public Individual(PaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        public T Accept<T>(IPaymentTypeVisitor<T> visitor)
        {
            return visitor.VisitIndividual(paymentService);
        }

        public override bool Equals(object obj)
        {
            return obj is Individual individual &&
                   EqualityComparer<PaymentService>.Default.Equals(paymentService, individual.paymentService);
        }

        public override int GetHashCode()
        {
            return 1075050752 + EqualityComparer<PaymentService>.Default.GetHashCode(paymentService);
        }
    }
}
