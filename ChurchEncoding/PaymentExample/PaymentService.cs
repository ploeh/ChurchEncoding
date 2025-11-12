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
            Name = name;
            Action = action;
        }

        public string Name { get; }

        public string Action { get; }

        public override bool Equals(object obj)
        {
            return obj is PaymentService service &&
                   Name == service.Name &&
                   Action == service.Action;
        }

        public override int GetHashCode()
        {
            int hashCode = 714674965;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Action);
            return hashCode;
        }
    }
}
