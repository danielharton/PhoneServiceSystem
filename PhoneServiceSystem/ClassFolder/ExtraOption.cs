using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneServiceSystem.ClassFolder
{
    public class ExtraOption
    {
        public long ExtraOptionId { get; set; }
        public string Name { get; set; }
        public float MonthlyCost { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
