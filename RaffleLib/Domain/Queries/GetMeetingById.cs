using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;

namespace RaffleLib.Domain.Queries
{
    public class GetMeetingById : IQuery
    {
        private IEntityRepository<Meeting> _repo;
        public GetMeetingById(IEntityRepository<Meeting> repo)
        {
            _repo = repo;
        }

        public Meeting Result(Guid id)
        {
            return _repo.Query.Where(m => m.Id == id).FirstOrDefault();
        }
    }
}
