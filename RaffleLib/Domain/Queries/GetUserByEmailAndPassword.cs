using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;
using RaffleLib.Security;

namespace RaffleLib.Domain.Queries
{
    public class GetUserByEmailAndPassword : IGetUserByEmailAndPassword
    {
        IEntityRepository<Member> _repo;
        IHasher _hasher;
        public GetUserByEmailAndPassword(IEntityRepository<Member> repo, IHasher hasher)
        {
            _repo = repo;
            _hasher = hasher;
        }

        public Member Result(string email, string password)
        {
            string hashpass = _hasher.Hash(password);

            return _repo.Query
                .Where(m => m.Email == email && m.PasswordHash == hashpass)
                .SingleOrDefault();
        }
    }
}
