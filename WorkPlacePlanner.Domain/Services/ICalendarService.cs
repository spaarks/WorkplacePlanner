using System;
using System.Collections.Generic;
using System.Text;
using WorkPlacePlanner.Domain.Dtos.Calendar;

namespace WorkPlacePlanner.Domain.Services
{
    public interface ICalendarService
    {
        List<CalendarRawDto> GetCalendar(int teamId, DateTime month);

        List<UsageTypeDto> GetUsageTypes();

        void UpdateCalendar(CalendarUpdateDto data);
    }
}
