using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.ChurchEncoding.PaymentExample
{
    public class PaymentService
    {
        public PaymentService(string name, string action)
        {
            this.Name = name;
            this.Action = action;
        }

        public string Name { get; }

        public string Action { get; }
    }
}
