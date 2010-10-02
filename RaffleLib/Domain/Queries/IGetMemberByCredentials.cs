using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;

namespace RaffleLib.Domain.Queries
{
    public interface IGetMemberByCredentials : IQuery
    {
        Member Result(string username, string password);
    }
}
