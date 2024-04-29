using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace ParkingOnBoard2._0
{
    public class ParkingSlot
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int? StreetId { get; set; }
        public bool IsClosed { get; set; }

        public bool IsValid { get; set; }


        [ForeignKey("StreetId")]
        public virtual Street? Street { get; set; }
    }
}
