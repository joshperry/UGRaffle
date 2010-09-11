using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;

namespace RaffleLib.Domain.Queries
{
    public class GetMemberByEmail : IGetMemberByEmail
    {
        private IEntityRepository<Member> _repo;
        public GetMemberByEmail(IEntityRepository<Member> repo)
        {
            _repo = repo;
        }

        public Member Result(string email)
        {
            return _repo.Query.Where(m => m.Email == email).FirstOrDefault();
        }
    }
}
