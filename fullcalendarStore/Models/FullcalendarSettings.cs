using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fullcalendarStore.Models
{
    public class FullcalendarSettings
    {
        public string ItemsUrl { get; set; }
        public string StartDate { get; set; }
        public decimal AspectRatio { get; set; }
        public int FirstDay { get; set; }
        public int FirstHour { get; set; }
        public bool EnableEditing { get; set; }
    }
}
