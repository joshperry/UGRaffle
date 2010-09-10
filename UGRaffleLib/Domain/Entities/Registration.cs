using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaffleLib.Domain.Entities
{
    public class Registration
    {
        public virtual Guid Id { get; set; }
        public virtual Member Member { get; set; }
        public virtual Meeting Meeting { get; set; }
        public virtual DateTime Time { get; set; }
    }
}
