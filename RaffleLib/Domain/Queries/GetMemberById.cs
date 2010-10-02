using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;

namespace RaffleLib.Domain.Queries
{
    public class GetMemberById : IGetMemberById
    {
        private IEntityRepository<Member> _repo;
        public GetMemberById(IEntityRepository<Member> repo)
        {
            _repo = repo;
        }

        public Entities.Member Result(Guid Id)
        {
            return _repo.Query.Where(m => m.Id == Id).FirstOrDefault();
        }
    }
}
