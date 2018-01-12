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

        public T Match<T>(
            Func<PaymentService, T> individual,
            Func<PaymentService, T> parent,
            Func<ChildPaymentService, T> child)
        {
            return child(childPaymentService);
        }
    }
}
