using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;

namespace RaffleLib.Domain.Queries
{
    public class GetCurrentMeetingAndRaffleItems : IGetCurrentMeetingAndRaffleItems
    {
        private IEntityRepository<Meeting> _repo;
        public GetCurrentMeetingAndRaffleItems(IEntityRepository<Meeting> repo)
        {
            _repo = repo;
        }

        public Meeting Result()
        {
            return _repo.Query
                .Where(m => m.Date == DateTime.Today)
                .SingleOrDefault();
        }
    }
}
