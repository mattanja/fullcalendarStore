using fullcalendarStore.Configuration;
using fullcalendarStore.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace fullcalendarStore.Data
{
    public class CalendarProxyService
    {
        private IEnumerable<CalendarItem> calendarItems = new CalendarItem[] { };
        private IAppSettings appSettings;
        private Timer refreshTimer;

        public CalendarProxyService(IAppSettings appSettings)
        {
            this.appSettings = appSettings;

            // Update now and every 1h
            refreshTimer = new Timer(this.RefreshTimerCallback, null, 0, 1000*60*60);
        }

        private void RefreshTimerCallback(object state)
        {
            this.TryUpdateCalendarItemsCache();
        }

        public IEnumerable<CalendarItem> GetCalendarItems(DateTime? start, DateTime? end)
        {
            var query = this.calendarItems.AsQueryable();

            if (start.HasValue)
            {
                query = query.Where(x => x.End >= start.Value);
            }

            if (end.HasValue)
            {
                query = query.Where(x => x.Start <= end.Value);
            }

            return query;
        }

        public void TryUpdateCalendarItemsCache()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = "YYYY-MM-DD",
                    ContractResolver = new CustomContractResolver()
                };

                using (WebClient wc = new WebClient())
                {
                    var json = wc.UploadString(appSettings.CalendarProxyFetchUrl, "");
                    var response = JsonConvert.DeserializeObject<CtResponse>(json, settings);

                    if (response.Status == "success")
                    {
                        this.calendarItems = response.Data;
                    }
                    else
                    {
                        Console.Write($"Error getting calendar items, Status {response.Status}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        public IEnumerable<CalendarItem> CalendarItems
        {
            get
            {
                return this.calendarItems;
            }
        }
    }

    public class CtResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "data")]
        public IEnumerable<CalendarItem> Data { get; set; }
    }

    public class CustomContractResolver : DefaultContractResolver
    {
        private Dictionary<string, string> PropertyMappings { get; set; }

        public CustomContractResolver()
        {
            this.PropertyMappings = new Dictionary<string, string>
            {
                {"Start", "startdate"},
                {"End", "enddate"},
                {"Title", "bezeichnung"},
            };
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            var resolved = this.PropertyMappings.TryGetValue(propertyName, out string resolvedName);
            return (resolved) ? resolvedName : base.ResolvePropertyName(propertyName);
        }
    }
}
