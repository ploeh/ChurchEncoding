using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding.PaymentExample
{
    public static class PaymentType
    {
        public static PaymentJsonModel ToJson(this IPaymentType payment)
        {
            return payment.Accept(new PaymentTypeToJsonVisitor());
        }

        private class PaymentTypeToJsonVisitor :
            IPaymentTypeVisitor<PaymentJsonModel>
        {
            public PaymentJsonModel VisitIndividual(PaymentService individual)
            {
                return new PaymentJsonModel
                {
                    Name = individual.Name,
                    Action = individual.Action,
                    StartRecurrent = new ChurchFalse(),
                    TransactionKey = new Nothing<string>()
                };
            }

            public PaymentJsonModel VisitParent(PaymentService parent)
            {
                return new PaymentJsonModel
                {
                    Name = parent.Name,
                    Action = parent.Action,
                    StartRecurrent = new ChurchTrue(),
                    TransactionKey = new Nothing<string>()
                };
            }

            public PaymentJsonModel VisitChild(ChildPaymentService child)
            {
                return new PaymentJsonModel
                {
                    Name = child.PaymentService.Name,
                    Action = child.PaymentService.Action,
                    StartRecurrent = new ChurchFalse(),
                    TransactionKey =
                        new Just<string>(child.OriginalTransactionKey)
                };
            }
        }
    }
}
