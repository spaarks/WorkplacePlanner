using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkPlacePlanner.Domain.Dtos.Calendar;
using WorkPlacePlanner.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace WorkplacePlanner.WebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Calendar")]
    public class CalendarController : Controller
    {
        ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }
        
        [HttpGet("{teamId}/{month}")]
        public IEnumerable<CalendarRawDto> Get(int teamId, DateTime month)
        {
            var calendarRows = _calendarService.GetCalendar(teamId, month);
            return calendarRows;
        }
        
        [HttpGet("Entries/{teamMembershipId}/{month}")]
        public IEnumerable<CalendarEntryDto> GetCalendarEntries(int teamMembershipId, DateTime month)
        {
            var calendarRows = _calendarService.GetCalendarEntries(teamMembershipId, month);
            return calendarRows;
        }

        [HttpPut]
        public void Put([FromBody]CalendarUpdateDto data)
        {
            _calendarService.UpdateCalendar(data);
        }

        [HttpGet("UsageTypes")]
        public IEnumerable<UsageTypeDto> GetUsagesType()
        {
            var usageTypes = _calendarService.GetUsageTypes();
            return usageTypes;
        }
    }
}
