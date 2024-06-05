using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Toll_Calculator_AFRY_JH.Project;
using Toll_Calculator_AFRY_JH.Project.Models;
using Xunit;

namespace TollCalculator.Tests
{
    public class AdditionalFeeCalculatorTests
    {
        private readonly HolidayManager _holidayManager;
        private readonly FeeCalculator _feeCalculator;

        public AdditionalFeeCalculatorTests()
        {
            _holidayManager = new HolidayManager();
            _feeCalculator = new FeeCalculator(_holidayManager);
            _holidayManager.InitializeAsync().Wait();
        }

        [Fact]
        public async Task CalculateFee_ForMultipleVehiclesOnNonHoliday_ReturnsExpectedFees()
        {
            // Arrange
            var car = new Car();
            var motorbike = new Motorbike();
            DateTime[] dates =
            {
                new DateTime(2024, 6, 7, 8, 0, 0),
                new DateTime(2024, 6, 7, 9, 0, 0)
            };

            // Act
            int carFee = await _feeCalculator.GetTollFeeAsync(car, dates);
            int motorbikeFee = await _feeCalculator.GetTollFeeAsync(motorbike, dates);

            // Debug output
            Console.WriteLine($"Car Fee: {carFee}");
            Console.WriteLine($"Motorbike Fee: {motorbikeFee}");

            // Assert
            Assert.Equal(21, carFee); // Expected fee for car based on provided dates and rates
            Assert.Equal(0, motorbikeFee); // Motorbikes should be toll-free
        }

        [Fact]
        public async Task CalculateFee_ForCarWithMultipleEntriesInOneHour_ReturnsMaxSingleFee()
        {
            // Arrange
            var vehicle = new Car();
            DateTime[] dates = 
            {
                new DateTime(2024, 6, 7, 8, 0, 0),
                new DateTime(2024, 6, 7, 8, 30, 0),
                new DateTime(2024, 6, 7, 8, 45, 0)
            };

            // Act
            int totalFee = await _feeCalculator.GetTollFeeAsync(vehicle, dates);

            // Debug output
            Console.WriteLine($"Total Fee: {totalFee}");

            // Assert
            Assert.Equal(13, totalFee); // The correct expected fee for the given time range
        }
    }
}