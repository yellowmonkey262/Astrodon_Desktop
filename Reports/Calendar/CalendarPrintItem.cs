using System;

namespace Astrodon.Reports.Calendar
{
    internal class CalendarPrintItem
    {
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public string Event { get; set; }
        public DateTime EventDate { get; set; }
        public string Venue { get; set; }
    }
}