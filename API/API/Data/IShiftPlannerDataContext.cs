using API.Models;
using System;
using System.Data.Entity;

namespace API.Data
{
    public interface IShiftPlannerDataContext : IDisposable
    {
        DbSet<User> Users { get; set; }
    }
}