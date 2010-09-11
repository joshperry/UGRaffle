using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;

namespace RaffleLib.Domain.Queries
{
    public class GetAllMeetings : IGetAllMeetings
    {
        private IEntityRepository<Meeting> _repo;
        public GetAllMeetings(IEntityRepository<Meeting> repo)
        {
            _repo = repo;
        }

        public IQueryable<Meeting> Result(int pagesize = 100, int pagenum = 1)
        {
            return _repo.Query.Skip(pagesize * (pagenum-1)).Take(pagesize);
        }
    }
}
