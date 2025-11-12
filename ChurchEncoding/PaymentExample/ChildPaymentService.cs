using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding.PaymentExample
{
    public class ChildPaymentService
    {
        public ChildPaymentService(
            string originalTransactionKey,
            PaymentService paymentService)
        {
            OriginalTransactionKey = originalTransactionKey;
            PaymentService = paymentService;
        }

        public string OriginalTransactionKey { get; }

        public PaymentService PaymentService { get; }

        public override bool Equals(object obj)
        {
            return obj is ChildPaymentService service &&
                   EqualityComparer<PaymentService>.Default.Equals(PaymentService, service.PaymentService);
        }

        public override int GetHashCode()
        {
            return -71619744 + EqualityComparer<PaymentService>.Default.GetHashCode(PaymentService);
        }
    }
}
