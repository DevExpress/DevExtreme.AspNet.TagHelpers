using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DevExtreme.AspNet.TagHelpers;

namespace Samples.Controllers {

    public class GoogleCalendarController {

        [HttpGet]
        public async Task<object> Appointments(DataSourceLoadOptions loadOptions) {
            var url = "https://www.googleapis.com/calendar/v3/calendars/"
                + "f7jnetm22dsjc3npc2lu3buvu4@group.calendar.google.com"
                + "/events?key=" + "AIzaSyBnNAISIUKe6xdhq1_rjor2rxoI3UlMY7k";

            using(var client = new HttpClient()) {
                var json = await client.GetStringAsync(url);
                var appointments = from i in JObject.Parse(json)["items"]
                                   select new {
                                       text = (string)i["summary"],
                                       startDate = (DateTime?)i["start"]["dateTime"],
                                       endDate = (DateTime?)i["end"]["dateTime"]
                                   };

                return DataSourceLoader.Load(appointments, loadOptions);
            }
        }
    }

}
