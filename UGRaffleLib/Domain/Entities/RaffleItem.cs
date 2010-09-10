using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaffleLib.Domain.Entities
{
    public class RaffleItem
    {
        public virtual Guid Id { get; set; }
        public virtual string Description { get; set; }
        public virtual int MinimumTickets { get; set; }
        public virtual byte[] Image { get; set; }
        public virtual Meeting Meeting { get; set; }
        public virtual Member Winner { get; set; }
    }
}
