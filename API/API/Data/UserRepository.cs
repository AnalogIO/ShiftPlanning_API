using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using API.Models.DTO;

namespace API.Data
{
    public class UserRepository : IUserRepository, IDisposable
    {

        private IShiftPlannerDataContext _context;

        public UserRepository()
        {
            _context = new ShiftPlannerDataContext();
        }
        public User Create(RegisterDTO userDto)
        {
            var existingUser = _context.Users.Where(x => x.Email == userDto.Email).FirstOrDefault();
            if (existingUser == null)
            {
                var user = new User { Email = userDto.Email, FirstName = userDto.FirstName, LastName = userDto.LastName, Title = userDto.Title };
                _context.Users.Add(user);
                _context.SaveChanges();
                return user;
            }
            else
            {
                return null;
            }
        }

        public void Delete(int id)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            if(user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public List<User> Read()
        {
            return _context.Users.ToList();
        }

        public User Read(int id)
        {
            return _context.Users.Where(x => x.Id == id).FirstOrDefault();
        }

        public int Update(User user)
        {
            _context.Users.Attach(user);
            _context.MarkAsModified(user);
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}