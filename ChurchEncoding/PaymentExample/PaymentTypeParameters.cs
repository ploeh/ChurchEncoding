using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding.PaymentExample
{
    public class PaymentTypeParameters<T>
    {
        public PaymentTypeParameters(
            Func<PaymentService, T> individual,
            Func<PaymentService, T> parent,
            Func<ChildPaymentService, T> child)
        {
            RunIndividual = individual;
            RunParent = parent;
            RunChild = child;
        }

        public Func<PaymentService, T> RunIndividual { get; }
        public Func<PaymentService, T> RunParent { get; }
        public Func<ChildPaymentService, T> RunChild { get; }
    }
}
