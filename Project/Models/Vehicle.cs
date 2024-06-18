using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toll_Calculator_AFRY_JH.Project.Models
{
    public abstract class Vehicle
    {
        public string Type { get; set; }

        public virtual string GetVehicleType()
        {
            return Type;
        }
    }
}