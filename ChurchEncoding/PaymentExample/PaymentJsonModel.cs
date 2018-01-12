using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding.PaymentExample
{
    public class PaymentJsonModel
    {
        public string Name { get; set; }

        public string Action { get; set; }

        public IChurchBoolean StartRecurrent { get; set; }

        public IMaybe<string> TransactionKey { get; set; }
    }
}
