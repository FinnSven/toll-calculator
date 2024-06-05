using System.Collections.Concurrent;
using PublicHoliday;
namespace Toll_Calculator_AFRY_JH.Project
{
    public class HolidayManager // purpose is to give the program the capacity to update when public holidays fall
    {
        private readonly ConcurrentDictionary<int, SwedenPublicHoliday> publicHolidaysStrategy;
        public HolidayManager()
        {
            publicHolidaysStrategy = new ConcurrentDictionary<int, SwedenPublicHoliday>();
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
            publicHolidaysStrategy.AddOrUpdate(year, holidaysStrategy, (key, oldValue) => holidaysStrategy);
        }
        public async Task<bool> IsPublicHolidayAsync(DateTime date)
        {
            if (!publicHolidaysStrategy.ContainsKey(date.Year))
            {
                await LoadHolidaysAsync(date.Year);
            }

            return publicHolidaysStrategy.TryGetValue(date.Year, out var holidayStrategy)
                   && holidayStrategy?.IsPublicHoliday(date) == true;
        }

        private SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        public async Task<IEnumerable<DateTime>> GetPublicHolidaysAsync(int year)
        {
            if (!publicHolidaysStrategy.TryGetValue(year, out var holidayStrategy))
            {
                await _lock.WaitAsync();
                try
                {
                    if (!publicHolidaysStrategy.TryGetValue(year, out holidayStrategy))
                    {
                        await LoadHolidaysAsync(year);
                        holidayStrategy = publicHolidaysStrategy[year];
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