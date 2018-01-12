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

        public T Match<T>(PaymentTypeParameters<T> parameters)
        {
            return parameters.Child(childPaymentService);
        }
    }
}
