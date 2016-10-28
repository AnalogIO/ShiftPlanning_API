using API.Models;
using API.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Data
{
    public interface IManagerRepository
    {
        Manager Create(Manager manager);
        List<Manager> Read();
        Manager Read(int id);
        Manager Login(string username, string password);
        int Update(Manager manager);
        void Delete(int id);
    }
}