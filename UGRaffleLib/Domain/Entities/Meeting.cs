using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RaffleLib.Domain.Entities
{
    public class Meeting
    {
        public virtual Guid Id { get; set; }

        [Required(ErrorMessage="Please supply a date")]
        [DataType(DataType.Date)]
        public virtual DateTime Date { get; set; }

        public virtual string Description { get; set; }
        
        public virtual int TicketsForRegistering { get; set; }
        
        public virtual IList<RaffleItem> RaffleItems { get; set; }
    }
}
