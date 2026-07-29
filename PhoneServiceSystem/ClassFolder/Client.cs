using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneServiceSystem.ClassFolder
{
    public class Client
    {
        public long ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
