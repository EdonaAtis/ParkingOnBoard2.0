using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ParkingOnBoard2._0
{
    public class ParkingContext : DbContext
    {
        public DbSet<Street> Streets { get; set; }
        public DbSet<ParkingSlot> ParkingSlots { get; set; }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-0DAPEMA\\MSSQLSERVER01;Database=MSSQLSERVER01;Integrated Security=true;");
        }



    }
}
