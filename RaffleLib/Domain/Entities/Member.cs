using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaffleLib.Domain.Entities
{
    public class Member
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual int TicketCount { get; set; }
        public virtual IList<string> Roles { get; set; }
    }
}
