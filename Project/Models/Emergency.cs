
namespace Toll_Calculator_AFRY_JH.Project.Models
{
    public class Emergency : Vehicle
    {
        public override String GetVehicleType()
   
        {
            return nameof(Emergency); // Removed manual typing of string and implemented usage of nameof keyword.
        }
    }
}