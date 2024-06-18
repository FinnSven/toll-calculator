using Toll_Calculator_AFRY_JH.Project.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Toll_Calculator_AFRY_JH.Project
{
    public class FeeCalculator
    {
        private readonly HolidayManager _holidayManager;

        public FeeCalculator(HolidayManager holidayManager)
        {
            _holidayManager = holidayManager;
        }

        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */
        public async Task<int> GetTollFeeAsync(Vehicle vehicle, DateTime[] dates)
        {
            if (dates == null || dates.Length == 0)
            {
                throw new ArgumentException("Dates array is null or empty");
            }

            if (await IsTollFreeVehicleAsync(vehicle))
            {
                Console.WriteLine($"Vehicle {vehicle.GetVehicleType()} is toll-free.");
                return 0;
            }

            DateTime intervalStart = dates[0];
            int totalFee = 0;
            int currentHourFee = 0;

            foreach (DateTime date in dates)
            {
                int nextFee = await GetTollFeeAsync(date, vehicle);
                Console.WriteLine($"Date: {date}, Next Fee: {nextFee}");

                if (date >= intervalStart.AddHours(1))
                {
                    totalFee += currentHourFee;
                    intervalStart = date;
                    currentHourFee = nextFee; // Start new interval with the current fee
                }
                else
                {
                    currentHourFee = Math.Max(currentHourFee, nextFee); // Keep the maximum fee within the interval
                }
            }

            totalFee += currentHourFee; // Add the fee of the last interval

            if (totalFee > 60) totalFee = 60;

            Console.WriteLine($"Total Fee: {totalFee}");
            return totalFee;
        }

        private Task<bool> IsTollFreeVehicleAsync(Vehicle vehicle)
        {
            if (vehicle == null) return Task.FromResult(false);
            string vehicleType = vehicle.GetVehicleType();
            return Task.FromResult(vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
                                   vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
                                   vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
                                   vehicleType.Equals(TollFreeVehicles.Diplomatic.ToString()) ||
                                   vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
                                   vehicleType.Equals(TollFreeVehicles.Military.ToString()));
        }

        private async Task<int> GetTollFeeAsync(DateTime date, Vehicle vehicle)
        {
            if (await IsTollFreeDateAsync(date) || await IsTollFreeVehicleAsync(vehicle))
            {
                Console.WriteLine($"Date {date} or vehicle {vehicle.GetVehicleType()} is toll-free.");
                return 0;
            }

            int hour = date.Hour;
            int minute = date.Minute;

            int cost;
            switch (hour) // used a switch method to work through the logic 
            {
                case 6:
                    cost = (minute <= 29) ? 8 : 13;
                    break;
                case 7:
                    cost = 18;
                    break;
                case 8:
                    cost = (minute <= 29) ? 13 : 8;
                    break;
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                    cost = 8;
                    break;
                case 14:
                    cost = (minute <= 29) ? 8 : 13;
                    break;
                case 15:
                    cost = (minute <= 29) ? 13 : 18;
                    break;
                case 16:
                    cost = 18;
                    break;
                case 17:
                    cost = 13;
                    break;
                case 18:
                    cost = (minute <= 29) ? 8 : 0;
                    break;
                default:
                    cost = 0;
                    break;
            }

            Console.WriteLine($"Date: {date}, Hour: {hour}, Minute: {minute}, Cost: {cost}");
            return cost;
        }

        private async Task<bool> IsTollFreeDateAsync(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                Console.WriteLine($"Date {date} is a weekend.");
                return true;
            }

            bool isHoliday = await _holidayManager.IsPublicHolidayAsync(date);
            Console.WriteLine($"Date {date} is a public holiday: {isHoliday}");
            return isHoliday;
        }
    }
}