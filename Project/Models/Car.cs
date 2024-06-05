using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Toll_Calculator_AFRY_JH.Project.Models
{
    public class Car : Vehicle
    {
        public Car()
        {
            Type = "Car";
        }

        public override string GetVehicleType()
        {
            return Type;
        }
    }
}
  
