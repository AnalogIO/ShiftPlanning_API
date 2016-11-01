using API.Models;
using API.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Models;

namespace API.Logic
{
    public static class Mapper
    {
        public static ManagerLoginResponse ManagerToLoginResponse(Manager manager)
        {
            var managerDto = new ManagerLoginResponse { Token = manager.Tokens.LastOrDefault().TokenHash };
            return managerDto;
        }
    }
}