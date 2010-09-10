using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;

namespace RaffleLib.Domain.Repositories.Fake
{
    public class FakeMeetingRepository : IEntityRepository<Meeting>
    {
        private IList<Meeting> _meetings = new List<Meeting>
        {
            new Meeting {
                Date = DateTime.Now,
                Description = "This is our current meeting, I hope you enjoy it immensely.",
                RaffleItems = new List<RaffleItem> {
                    new RaffleItem {
                        Description = "Laptop Bag",
                        MinimumTickets = 5
                    }
                }
            }
        };

        public IQueryable<Meeting> Query
        {
            get { return _meetings.AsQueryable(); }
        }

        public void Save(Meeting entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Meeting entity)
        {
            throw new NotImplementedException();
        }
    }
}
