using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;

namespace RaffleLib.Domain.Queries
{
    public class GetMemberRegistration : IGetMemberRegistration
    {
        private IEntityRepository<Registration> _repo;
        public GetMemberRegistration(IEntityRepository<Registration> regRepo)
        {
            _repo = regRepo;
        }

        public Registration Result(Meeting meeting, Member member)
        {
            return _repo.Query.Where(r => r.Member == member && r.Meeting == meeting).FirstOrDefault();
        }
    }
}
