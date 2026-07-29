using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneServiceSystem.ClassFolder
{
    public class Subscription
    {
        public long SubscriptionId { get; set; }
        public long ClientId { get; set; }
        public long ExtraOptionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual Client Client { get; set; }
        public virtual ExtraOption ExtraOption { get; set; }
    }
}
