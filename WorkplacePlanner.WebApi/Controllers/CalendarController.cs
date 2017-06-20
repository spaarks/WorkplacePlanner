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

        
        [HttpGet(Name = "Get")]
        public IEnumerable<CalendarRawDto> Get(int teamId, DateTime month)
        {
            var calendarRows = _calendarService.GetCalendar(teamId, month);
            return calendarRows;
        }

        [HttpGet(Name = "GetMetaData")]
        public CalendarMetaDataDto GetMetaData(int teamId, DateTime month)
        {
            var metaData = _calendarService.GetCalendarMetaData(teamId, month);
            return metaData;
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
