using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaffleLib.Domain.Entities
{
    public class RaffleEntry
    {
        public virtual Guid Id { get; set; }
        public virtual RaffleItem RaffleItem { get; set; }
        public virtual Member Member { get; set; }
    }
}
