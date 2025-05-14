using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlanner
{
    public class Event
    {
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }

        public Event(string eventName, DateTime eventDate)
        {
            EventName = eventName;
            EventDate = eventDate;
        }
        public string DaysUntilOrAfterEvent()
        {
            TimeSpan daysUntilEvent = EventDate - DateTime.Now;
            TimeSpan daysAfterEvent = DateTime.Now - EventDate;

            if (daysUntilEvent.TotalDays > 0)
            {
                return $"will happen in {daysUntilEvent.Days} days.";
            }
            else
            {
                return $"happened {daysAfterEvent.Days} days ago.";
            }
        }

    }
}
