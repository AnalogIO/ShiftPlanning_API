using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.DTO
{
    public class CreateScheduleDTO
    {
        public string Name { get; set; }
        public int NumberOfWeeks { get; set; }
    }
}