using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLayer
{
    public class Meeting
    {
        public int MeetingId { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public List<Contact> Attendees { get; set; }
        public Dictionary<Contact, MeetingInsights> Insights { get; set; }

        public Meeting(int meetingId, string title, DateTime startTime, List<Contact> attendees)
        {
            MeetingId = meetingId;
            Title = title;
            StartTime = startTime;
            Attendees = attendees;
            Insights = new Dictionary<Contact, MeetingInsights>();
            foreach (var c in Attendees)
            {
                Insights.Add(c, new MeetingInsights());
            }
        }
    }
}
