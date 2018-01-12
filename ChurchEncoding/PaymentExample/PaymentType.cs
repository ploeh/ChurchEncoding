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
            return payment.Match(
                new PaymentTypeParameters<PaymentJsonModel>(
                    individual : ps =>
                        new PaymentJsonModel
                        {
                            Name = ps.Name,
                            Action = ps.Action,
                            StartRecurrent = new ChurchFalse(),
                            TransactionKey = new Nothing<string>()
                        },
                    parent : ps =>
                        new PaymentJsonModel
                        {
                            Name = ps.Name,
                            Action = ps.Action,
                            StartRecurrent = new ChurchTrue(),
                            TransactionKey = new Nothing<string>()
                        },
                    child : cps =>
                        new PaymentJsonModel
                        {
                            Name = cps.PaymentService.Name,
                            Action = cps.PaymentService.Action,
                            StartRecurrent = new ChurchFalse(),
                            TransactionKey =
                                new Just<string>(cps.OriginalTransactionKey)
                        }));
        }
    }
}
