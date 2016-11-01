using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Npgsql.Mapping;
using Data.Repositories;
using Data.Token;
using PGManager = Data.Npgsql.Models.Manager;
using PGToken = Data.Npgsql.Models.Token;

namespace Data.Npgsql.Repositories
{
    public class ManagerRepository : IManagerRepository, IDisposable
    {
        private readonly IShiftPlannerDataContext _context;
        private readonly IMapper<Manager, PGManager> _managerMapper;
        private readonly IMapMany<PGManager, Manager> _managerMapMany;
        
        public ManagerRepository(IShiftPlannerDataContext context, 
            IMapper<Manager, PGManager> managerMapper, 
            IMapMany<PGManager, Manager> managerMapMany)
        {
            _context = context;
            _managerMapper = managerMapper;
            _managerMapMany = managerMapMany;
        }

        public Manager Create(Manager manager)
        {
            if (!_context.Managers.Any(x => x.Username == manager.Username))
            {
                var newManager = _managerMapper.MapToEntity(manager);

                _context.Managers.Add(newManager);
                _context.SaveChanges();
                return _managerMapper.MapToModel(newManager);
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
            return _managerMapMany.Map(_context.Managers);
        }

        public Manager Read(int id)
        {
            return _managerMapper.MapToModel(_context.Managers.FirstOrDefault(x => x.Id == id));
        }

        public Manager Read(string token)
        {
            var manager = _context.Managers.FirstOrDefault(m => m.Tokens.Any(t => t.TokenHash == token)); // finds the manager with the corresponding token

            if (manager != null)
            {
                return _managerMapper.MapToModel(manager);
            }
            return null;
        }

        public int Update(Manager manager)
        {
            var dbManager = _context.Managers.Single(m => m.Id == manager.Id);

            dbManager.Password = manager.Password;
            dbManager.Salt = manager.Salt;
            dbManager.Tokens = manager.Tokens.Select(t => new PGToken(t.TokenHash)
            {
                Id = t.Id
            }).ToList();

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
                    var token = new PGToken(TokenManager.GenerateLoginToken());
                    manager.Tokens.Add(token);
                    _context.SaveChanges();
                    return _managerMapper.MapToModel(manager);
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