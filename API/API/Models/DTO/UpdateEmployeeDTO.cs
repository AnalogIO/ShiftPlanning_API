using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.DTO
{
    public class UpdateEmployeeDTO
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
}