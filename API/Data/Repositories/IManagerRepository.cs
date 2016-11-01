using System.Collections.Generic;
using Data.Models;

namespace Data.Repositories
{
    public interface IManagerRepository
    {
        Manager Create(Manager manager);
        IEnumerable<Manager> Read();
        Manager Read(int id);
        Manager Read(string token);
        Manager Login(string username, string password);
        int Update(Manager manager);
        void Delete(int id);
    }
}