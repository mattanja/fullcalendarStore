using fullcalendarStore.Configuration;
using fullcalendarStore.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace fullcalendarStore.Data
{
    public class CalendarProxyService
    {
        private IEnumerable<CalendarItem> calendarItems;
        private IAppSettings appSettings;

        public CalendarProxyService(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
            this.InitializeCalendarItemsCache();
        }

        private void InitializeCalendarItemsCache()
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
                        this.calendarItems = new List<CalendarItem>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                this.calendarItems = new List<CalendarItem>();
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
