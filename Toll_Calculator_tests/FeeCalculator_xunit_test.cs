using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Toll_Calculator_AFRY_JH.Project;
using Toll_Calculator_AFRY_JH.Project.Models;
using Xunit;

namespace TollCalculator.Tests
{
    public class FeeCalculatorTests
    {
        private readonly HolidayManager _holidayManager;
        private readonly FeeCalculator _feeCalculator;

        public FeeCalculatorTests()
        {
            _holidayManager = new HolidayManager();
            _feeCalculator = new FeeCalculator(_holidayManager);
            _holidayManager.InitializeAsync().Wait();
        }

        [Fact]
        public async Task CalculateFee_ForCarOnNonHoliday_ReturnsExpectedFee()
        {
            // Arrange
            var vehicle = new Car();
            DateTime[] dates =
            {
                new DateTime(2024, 6, 7, 8, 0, 0),
                new DateTime(2024, 6, 7, 9, 0, 0)
            };

            // Act
            int totalFee = await _feeCalculator.GetTollFeeAsync(vehicle, dates);

            // Debug output
            Console.WriteLine($"Total Fee: {totalFee}");

            // Assert
            Assert.Equal(21, totalFee); // Expected fee based on provided dates and rates
        }

        [Fact]
        public async Task CalculateFee_ForMotorbikeOnNonHoliday_ReturnsZero()
        {
            // Arrange
            var vehicle = new Motorbike();
            DateTime[] dates =
            {
                new DateTime(2024, 6, 6, 8, 0, 0),
                new DateTime(2024, 6, 6, 9, 0, 0)
            };

            // Act
            int totalFee = await _feeCalculator.GetTollFeeAsync(vehicle, dates);

            // Assert
            Assert.Equal(0, totalFee); // Motorbikes should be toll-free
        }

        [Fact]
        public async Task CalculateFee_ForCarOnHoliday_ReturnsZero()
        {
            // Arrange
            var vehicle = new Car();
            DateTime[] dates =
            {
                new DateTime(2024, 1, 1, 8, 0, 0), // Assuming January 1st is a holiday
                new DateTime(2024, 1, 1, 9, 0, 0)
            };

            // Act
            int totalFee = await _feeCalculator.GetTollFeeAsync(vehicle, dates);

            // Assert
            Assert.Equal(0, totalFee); // No fee should be charged on holidays
        }
    }
}