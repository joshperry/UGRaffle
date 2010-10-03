using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RaffleLib.Domain.Entities;

namespace RaffleWeb.Models
{
    public class IndexViewModel
    {
        public Meeting Meeting { get; set; }
        public Member Member { get; set; }
    }
}