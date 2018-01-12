using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding.PaymentExample
{
    public interface IPaymentTypeParameters<T>
    {
        T RunIndividual(PaymentService individual);
        T RunParent(PaymentService parent);
        T RunChild(ChildPaymentService child);
    }
}
