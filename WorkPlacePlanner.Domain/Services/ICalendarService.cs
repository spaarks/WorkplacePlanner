using System;
using System.Collections.Generic;
using System.Text;
using WorkPlacePlanner.Domain.Dtos.Calendar;

namespace WorkPlacePlanner.Domain.Services
{
    public interface ICalendarService
    {
        List<CalendarRawDto> GetCalendar(int teamId, DateTime month);
       
        void UpdateCalendar(CalendarUpdateDto data);

        CalendarMetaDataDto GetCalendarMetaData(int teamId, DateTime date);
    }
}
