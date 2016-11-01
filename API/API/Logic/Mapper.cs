using System.Linq;
using Data.Models;
using DataTransferObjects;

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