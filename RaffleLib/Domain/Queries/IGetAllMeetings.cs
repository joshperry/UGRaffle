﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;

namespace RaffleLib.Domain.Queries
{
    public interface IGetAllMeetings : IQuery
    {
        IQueryable<Meeting> Result(int pagesize = 100, int pagenum = 1);
    }
}
