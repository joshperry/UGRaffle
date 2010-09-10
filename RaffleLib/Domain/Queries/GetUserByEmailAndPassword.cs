using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;
using System.Security.Cryptography;

namespace RaffleLib.Domain.Queries
{
    public class GetUserByEmailAndPassword : IQuery
    {
        IEntityRepository<Member> _repo;
        public GetUserByEmailAndPassword(IEntityRepository<Member> repo)
        {
            _repo = repo;
        }

        public Member Result(string email, string password)
        {
            string hashpass = BitConverter.ToString(
                new SHA256CryptoServiceProvider()
                .ComputeHash(System.Text.Encoding.ASCII.GetBytes(password))
                ).Replace("-", "").ToLowerInvariant();

            return _repo.Query
                .Where(m => m.Email == email && m.PasswordHash == hashpass)
                .SingleOrDefault();
        }
    }
}
