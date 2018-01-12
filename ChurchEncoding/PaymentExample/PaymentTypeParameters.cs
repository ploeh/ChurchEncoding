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
            Individual = individual;
            Parent = parent;
            Child = child;
        }

        public Func<PaymentService, T> Individual { get; }
        public Func<PaymentService, T> Parent { get; }
        public Func<ChildPaymentService, T> Child { get; }
    }
}
