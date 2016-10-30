﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class EmployeeTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual Institution Institution { get; set; }
    }
}