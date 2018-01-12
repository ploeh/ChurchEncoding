using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding.PaymentExample
{
    public interface IPaymentTypeVisitor<T>
    {
        T VisitIndividual(PaymentService individual);
        T VisitParent(PaymentService parent);
        T VisitChild(ChildPaymentService child);
    }
}
