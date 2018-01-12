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
            return payment.Match(new PaymentTypeToJsonParameters());
        }

        private class PaymentTypeToJsonParameters :
            IPaymentTypeParameters<PaymentJsonModel>
        {
            public PaymentJsonModel RunIndividual(PaymentService individual)
            {
                return new PaymentJsonModel
                {
                    Name = individual.Name,
                    Action = individual.Action,
                    StartRecurrent = new ChurchFalse(),
                    TransactionKey = new Nothing<string>()
                };
            }

            public PaymentJsonModel RunParent(PaymentService parent)
            {
                return new PaymentJsonModel
                {
                    Name = parent.Name,
                    Action = parent.Action,
                    StartRecurrent = new ChurchTrue(),
                    TransactionKey = new Nothing<string>()
                };
            }

            public PaymentJsonModel RunChild(ChildPaymentService child)
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
