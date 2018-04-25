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
        private DateTime rangeEnd;
        private IAppSettings appSettings;
        private Timer refreshTimer;

        public CalendarProxyService(IAppSettings appSettings)
        {
            this.appSettings = appSettings;

            // Update now and every 1h
            refreshTimer = new Timer(this.RefreshTimerCallback, null, 0, 1000 * 60 * 60);
        }

        private void RefreshTimerCallback(object state)
        {
            this.TryUpdateCalendarItemsCache();
        }

        public IEnumerable<CalendarItem> GetCalendarItems(DateTime? start, DateTime? end)
        {
            if (end.HasValue && this.rangeEnd < end.Value)
            {
                // Get new range
                var fromInt = (this.rangeEnd - DateTime.Today).Days;
                var toInt = (end.Value - this.rangeEnd).Days + fromInt;
                TryUpdateCalendarItemsCache(requestdata: $"from={fromInt}&to={toInt}", clearCache: false);
            }

            var query = this.calendarItems.AsQueryable();

            if (start.HasValue)
            {
                query = query.Where(x => x.End >= start.Value);
            }

            if (end.HasValue)
            {
                query = query.Where(x => x.Start < end.Value);
            }

            return query;
        }

        public void TryUpdateCalendarItemsCache(string requestdata = null, bool clearCache = true)
        {
            try
            {
                if (String.IsNullOrEmpty(requestdata))
                {
                    var fromInt = -(1+(int)DateTime.Today.DayOfWeek); // start with begin of week
                    requestdata = $"from={fromInt}&to=31";
                }

                var settings = new JsonSerializerSettings
                {
                    DateFormatString = "YYYY-MM-DD",
                    ContractResolver = new CustomContractResolver()
                };

                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    var json = wc.UploadString(appSettings.CalendarProxyFetchUrl, requestdata);
                    var response = JsonConvert.DeserializeObject<CtResponse>(json, settings);

                    if (response.Status == "success")
                    {
                        if (clearCache)
                        {
                            this.calendarItems = response.Data.OrderBy(x => x.Start);
                        }
                        else
                        {
                            this.calendarItems = this.calendarItems.Union(response.Data, new CalendarItemComparer());
                        }

                        this.rangeEnd = response.Data.Select(x => x.Start).Max(); // using start date on purpose
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

    internal class CalendarItemComparer : IEqualityComparer<CalendarItem>
    {
        public bool Equals(CalendarItem x, CalendarItem y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(CalendarItem obj)
        {
            return obj.Start.GetHashCode()*3 + obj.Title.GetHashCode()*5 + obj.End.GetHashCode()*7;
        }
    }
}
