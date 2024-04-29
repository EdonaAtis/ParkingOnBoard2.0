using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;



namespace ParkingOnBoard2._0
{
    public class Street
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int NumberOfSides { get; set; }
        public int TotalParkingSlots { get; set; }
        public bool IsClosed { get; set; }
    }

}
