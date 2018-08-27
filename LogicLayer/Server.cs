using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase;
using EntitiesLayer;

namespace LogicLayer
{
    public class Server : IServer
    {
        public IDB DB { get; set; }
        public Server()
        {
            DB = MockDB.GetInstance();
        }
        public List<Meeting> FindLastMeetingWithContact(Contact a, Contact b)
        {
            return DB.FindLastMeeting(a, b);
        }

        public Meeting FindNextMeeting(Contact contact)
        {
            return DB.FindNextMeeting(contact);
        }

        public Contact GetContactByName(string name)
        {
            return DB.GetContactByName(name);
        }

        public List<Tip> GetGivenTips(Meeting meeting, Contact contact)
        {
            MeetingInsights insights;
            if (meeting.Insights.TryGetValue(contact, out insights)) {
                return insights.Tips.Keys.ToList();
            }
            else
            {
                return new List<Tip>();
            }
        }

        public List<Tip> GetTipsAboutPersonForMeeting(Meeting meeting, Contact contact, bool isCloseToTheMeeting)
        {
            return DB.GetTipsAboutPersonForMeeting(meeting, contact, isCloseToTheMeeting);
        }

        public void RateTip(Meeting meeting, Contact contact, Tip tip, bool WasGood)
        {
            int rate = WasGood ? 1 : -1;
            int currRate;
            if (contact.TipsRate.TryGetValue(tip, out currRate)) {
                currRate = currRate + rate;
                contact.TipsRate.Remove(tip);
                contact.TipsRate.Add(tip, currRate);
            }

            MeetingInsights insights;
            if (meeting.Insights.TryGetValue(contact, out insights))
            {
                int tipRate;
                if (insights.Tips.TryGetValue(tip, out tipRate))
                {
                    tipRate = tipRate + rate;
                    insights.Tips.Remove(tip);
                    insights.Tips.Add(tip, tipRate);
                }
            }
        }

        public void SetMeetingPurpose(Meeting meeting, Contact contact, string purpose)
        {
            MeetingInsights insights;
            if (meeting.Insights.TryGetValue(contact, out insights))
            {
                insights.Purpose = purpose;
            }
        }

        public void SetMeetingSatisfaction(Meeting meeting, Contact contact, string satisfaction)
        {
            MeetingInsights insights;
            if (meeting.Insights.TryGetValue(contact, out insights))
            {
                insights.Satisfaction = satisfaction;
            }
        }
    }
}
