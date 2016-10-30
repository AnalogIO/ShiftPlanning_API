using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using API.Models.DTO;
using API.Logic;

namespace API.Data
{
    public class ManagerRepository : IManagerRepository, IDisposable
    {

        private IShiftPlannerDataContext _context;

        public ManagerRepository()
        {
            _context = new ShiftPlannerDataContext();
        }

        public ManagerRepository(ShiftPlannerDataContext context)
        {
            _context = context;
        }

        public Manager Create(Manager manager)
        {
            var existingManager = _context.Managers.Where(x => x.Username == manager.Username).FirstOrDefault();
            if (existingManager == null)
            {
                _context.Managers.Add(manager);
                _context.SaveChanges();
                return manager;
            }
            else
            {
                return null;
            }
        }

        public void Delete(int id)
        {
            var manager = _context.Managers.Where(x => x.Id == id).FirstOrDefault();
            if(manager != null)
            {
                _context.Managers.Remove(manager);
                _context.SaveChanges();
            }
        }

        public List<Manager> Read()
        {
            return _context.Managers.ToList();
        }

        public Manager Read(int id)
        {
            return _context.Managers.Where(x => x.Id == id).FirstOrDefault();
        }

        public Manager Read(string token)
        {
            return _context.Managers.Where(m => m.Tokens.Where(t => t.TokenHash == token).Count() > 0).FirstOrDefault(); // finds the manager with the corresponding token
        }

        public int Update(Manager manager)
        {
            _context.Managers.Attach(manager);
            _context.MarkAsModified(manager);
            return _context.SaveChanges();
        }

        public Manager Login(string username, string password)
        {
            var manager = _context.Managers.Where(m => m.Username == username).FirstOrDefault();
            if (manager != null)
            {
                var hashPassword = HashManager.Hash(password + manager.Salt);
                if (manager.Password.Equals(hashPassword))
                {
                    var token = new Token(TokenManager.GenerateLoginToken());
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