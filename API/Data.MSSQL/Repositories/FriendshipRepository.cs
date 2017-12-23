using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;
using System.Data.Entity;
using Data.Exceptions;
using System.Data;
using Data.Token;
using System.Data.Entity.Core;

namespace Data.MSSQL.Repositories
{
    public class FriendshipRepository : IFriendshipRepository, IDisposable
    {
        private readonly IShiftPlannerDataContext _context;

        public FriendshipRepository(IShiftPlannerDataContext context)
        {
            _context = context;
        }

        public Friendship CreateFrienship(Friendship friendship)
        {
            _context.Friendships.Add(friendship);
            return _context.SaveChanges() > 0 ? friendship : null;
        }

        public void DeleteFriendship(Friendship friendship)
        {
            _context.Friendships.Remove(friendship);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<Friendship> GetFriendships(int employeeId)
        {
            return _context.Friendships.Where(f => f.Employee_Id == employeeId);
        }
    }
}