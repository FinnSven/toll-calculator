// File: Project/IHolidayManager.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Toll_Calculator_AFRY_JH.Project
{
    public interface IHolidayManager
    {
        Task<bool> IsPublicHolidayAsync(DateTime date);
        Task<IEnumerable<DateTime>> GetPublicHolidaysAsync(int year);
        Task InitializeAsync();
    }
}