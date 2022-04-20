#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using aspnetcoreapp.Models;

namespace aspnetcoreapp.Data
{
    public class aspnetcoreappContext : DbContext
    {
        public aspnetcoreappContext (DbContextOptions<aspnetcoreappContext> options)
            : base(options)
        {
        }

        public DbSet<aspnetcoreapp.Models.Movie> Movie { get; set; }
    }
}
