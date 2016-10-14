using API.Models;
using API.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Data
{
    public interface IUserRepository
    {
        User Create(RegisterDTO user);
        List<User> Read();
        User Read(int id);
        int Update(User user);
        void Delete(int id);
    }
}