using System;
using System.Threading.Tasks;
using Toll_Calculator_AFRY_JH.Project;
using Toll_Calculator_AFRY_JH.Project.Models;
using DateTime = System.DateTime;

namespace Toll_Calculator_AFRY_JH.Project
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize the HolidayManager
            var holidayManager = new HolidayManager();
            await holidayManager.InitializeAsync();

            // Create an instance of FeeCalculator
            var feeCalculator = new FeeCalculator(holidayManager);

            // Example vehicle and dates
            var vehicle = new Car(); // Use the Car class here instead of Vehicle
            DateTime[] dates = 
            { 
                new DateTime(2024, 6, 6, 8, 0, 0), 
                new DateTime(2024, 6, 6, 9, 0, 0),
                new DateTime(2025,6,8,5,0,0),
                new DateTime(2026, 1, 14,17,30,00),
                new DateTime(2024,6,20,05,5,00)
            };
            
            
            // Calculate the total toll fee for the day
            int totalFee = await feeCalculator.GetTollFeeAsync(vehicle, dates);
            Console.WriteLine($"Total toll fee for the day: {totalFee}");
        }
    }
}
