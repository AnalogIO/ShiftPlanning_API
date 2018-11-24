using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IOrganizationRepository
    {
        bool Exists(string shortKey);
        bool Exists(int id);
        bool HasApiKey(string apiKey);
        Organization Read(int id);
        Organization Read(string apiKey);
        Organization ReadByShortKey(string shortKey);
    }
}
