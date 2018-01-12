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

        public T Match<T>(IPaymentTypeParameters<T> parameters)
        {
            return parameters.RunIndividual(paymentService);
        }
    }
}
