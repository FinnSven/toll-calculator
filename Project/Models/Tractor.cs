namespace Toll_Calculator_AFRY_JH.Project.Models
{
    public class Tractor : Vehicle
    {
        public override String GetVehicleType()
        {
            return nameof(Tractor); // Removed manual typing of string and implemented usage of nameof keyword.
        }
    }
}