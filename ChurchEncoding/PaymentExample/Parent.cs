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

        public T Match<T>(
            Func<PaymentService, T> individual,
            Func<PaymentService, T> parent,
            Func<ChildPaymentService, T> child)
        {
            return parent(paymentService);
        }
    }
}
