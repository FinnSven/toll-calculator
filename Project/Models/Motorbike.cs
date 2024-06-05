using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toll_Calculator_AFRY_JH.Project.Models
{
    public class Motorbike : Vehicle
    {
        public override String GetVehicleType()
        {
            return nameof(Motorbike); // Removed manual typing of string and implemented usage of nameof keyword.
        } 
    }
}