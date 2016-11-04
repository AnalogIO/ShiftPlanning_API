using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;
using Data.Token;

namespace Data.Npgsql.Repositories
{
    public class ManagerRepository : IManagerRepository, IDisposable
    {
        private readonly IShiftPlannerDataContext _context;
        
        public ManagerRepository(IShiftPlannerDataContext context)
        {
            _context = context;
        }

        public Manager Create(Manager manager)
        {
            if (!_context.Managers.Any(x => x.Username == manager.Username))
            {
                _context.Managers.Add(manager);
                _context.SaveChanges();
                return manager;
            }
            return null;
        }

        public void Delete(int id)
        {
            var manager = _context.Managers.FirstOrDefault(x => x.Id == id);
            if (manager != null)
            {
                _context.Managers.Remove(manager);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Manager> Read()
        {
            return _context.Managers.AsEnumerable();
        }

        public Manager Read(int id)
        {
            return _context.Managers.FirstOrDefault(x => x.Id == id);
        }

        public Manager Read(string token)
        {
            var manager = _context.Managers.FirstOrDefault(m => m.Tokens.Any(t => t.TokenHash == token)); // finds the manager with the corresponding token

            if (manager != null)
            {
                return manager;
            }
            return null;
        }

        public int Update(Manager manager)
        {
            var dbManager = _context.Managers.Single(m => m.Id == manager.Id);

            dbManager.Password = manager.Password;
            dbManager.Salt = manager.Salt;
            dbManager.Tokens = manager.Tokens;

            return _context.SaveChanges();
        }

        public Manager Login(string username, string password)
        {
            var manager = _context.Managers.FirstOrDefault(m => m.Username == username);
            if (manager != null)
            {
                var hashPassword = HashManager.Hash(password + manager.Salt);
                if (manager.Password.Equals(hashPassword))
                {
                    var token = new Models.Token(TokenManager.GenerateLoginToken());
                    manager.Tokens.Add(token);
                    _context.SaveChanges();
                    return manager;
                }
            }
            return null;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}