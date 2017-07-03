using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkPlacePlanner.Domain.Dtos.Calendar;
using WorkPlacePlanner.Domain.Services;

namespace WorkplacePlanner.WebApi.Controllers
{
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

        //[HttpGet]
        //[Route("MetaData")]
        //public CalendarMetaDataDto GetMetaData([FromQuery]int teamId, [FromQuery]DateTime month, [FromQuery]string x)
        //{
        //    var metaData = _calendarService.GetCalendarMetaData(teamId, month);
        //    return metaData;
        //}

        [HttpGet("UsageTypes")]
        public IEnumerable<UsageTypeDto> GetUsagesType()
        {
            var usageTypes = _calendarService.GetUsageTypes();
            return usageTypes;
        }

        /*
         
        // GET: api/Calendar
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Calendar/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Calendar
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Calendar/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
