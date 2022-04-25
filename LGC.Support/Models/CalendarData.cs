using System;

namespace LGC.Support.Models
{
    public class CalendarData
    {
        public int id { get; set; }
        public string title { get; set; }
        public string event_date { get; set; }
        public int remind_before_day { get; set; }
        public string description { get; set; }
        public string created_by { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now; 
        public string updated_by { get; set; }
        public DateTime updated_at { get; set; } = DateTime.Now;
    }
}
