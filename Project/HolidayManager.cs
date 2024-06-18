// File: Project/HolidayManager.cs
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PublicHoliday;

namespace Toll_Calculator_AFRY_JH.Project
{
    public class HolidayManager : IHolidayManager
    {
        private readonly ConcurrentDictionary<int, SwedenPublicHoliday> _publicHolidaysStrategy;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public HolidayManager()
        {
            _publicHolidaysStrategy = new ConcurrentDictionary<int, SwedenPublicHoliday>();
        }

        public async Task<bool> IsPublicHolidayAsync(DateTime date)
        {
            if (!_publicHolidaysStrategy.ContainsKey(date.Year))
            {
                await LoadHolidaysAsync(date.Year);
            }

            return _publicHolidaysStrategy.TryGetValue(date.Year, out var holidayStrategy)
                   && holidayStrategy?.IsPublicHoliday(date) == true;
        }

        public async Task InitializeAsync()
        {
            int currentYear = DateTime.UtcNow.Year;
            await Task.WhenAll(
                LoadHolidaysAsync(currentYear),
                LoadHolidaysAsync(currentYear + 1)
            );
        }

        private async Task LoadHolidaysAsync(int year)
        {
            var holidaysStrategy = new SwedenPublicHoliday();
            _publicHolidaysStrategy.AddOrUpdate(year, holidaysStrategy, (key, oldValue) => holidaysStrategy);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<DateTime>> GetPublicHolidaysAsync(int year)
        {
            if (!_publicHolidaysStrategy.TryGetValue(year, out var holidayStrategy))
            {
                await _lock.WaitAsync();
                try
                {
                    if (!_publicHolidaysStrategy.TryGetValue(year, out holidayStrategy))
                    {
                        await LoadHolidaysAsync(year);
                        holidayStrategy = _publicHolidaysStrategy[year];
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }

            return holidayStrategy.PublicHolidayNames(year).Keys;
        }
    }
}