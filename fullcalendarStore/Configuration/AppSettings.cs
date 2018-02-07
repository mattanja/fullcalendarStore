using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fullcalendarStore.Configuration
{
    public interface IAppSettings
    {
        /// <summary>
        /// Get configured initial display date
        /// </summary>
        String CalendarInitialDisplayDate { get; }

        /// <summary>
        /// First day of week (0=Sun, 1=Mon, etc.)
        /// </summary>
        int CalendarFirstDay { get; }
    }

    public class AppSettings : IAppSettings
    {
        private string calendarInitialDisplayDate = null;
        private int calendarFirstDay = 0;

        public AppSettings(IConfiguration configuration)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(configuration["CalendarInitialDisplayDate"]))
                {
                    calendarInitialDisplayDate = DateTime.Parse(configuration["CalendarInitialDisplayDate"]).ToString("yyyy-MM-dd");
                }

                if (!String.IsNullOrWhiteSpace(configuration["CalendarFirstDay"]))
                {
                    calendarFirstDay = int.Parse(configuration["CalendarFirstDay"]);
                }

            }
            catch (Exception)
            {
                // TODO error handling / logging
            }
        }

        public string CalendarInitialDisplayDate => calendarInitialDisplayDate ?? DateTime.Today.ToString("yyyy-MM-dd");

        public int CalendarFirstDay => calendarFirstDay;
    }
}
