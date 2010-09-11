using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;

namespace RaffleLib.Domain.Queries
{
    public class GetMembersInRole : IGetMembersInRole
    {
        private IEntityRepository<Member> _repo;
        public GetMembersInRole(IEntityRepository<Member> repo)
        {
            _repo = repo;
        }

        public IQueryable<Entities.Member> Result(string role)
        {
            return _repo.Query.Where(m => m.Roles.Contains(role));
        }
    }
}
