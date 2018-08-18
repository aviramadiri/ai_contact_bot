using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesLayer;

namespace LogicLayer
{
    public class Server : IServer
    {
        public List<Meeting> FindLastMeetingsWithContact(Contact a, Contact b)
        {
            throw new NotImplementedException();
        }

        public Meeting FindNextMeeting(Contact contact)
        {
            throw new NotImplementedException();
        }

        public Contact GetContactByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Tip> GetGivenTips(Meeting meeting, Contact contact)
        {
            throw new NotImplementedException();
        }

        public List<Tip> GetTipsAboutPersonForMeeting(Meeting meeting, Contact contact, bool isCloseToTheMeeting)
        {
            throw new NotImplementedException();
        }

        public void RateTip(Meeting meeting, Contact contact, Tip tip, bool WasGood)
        {
            throw new NotImplementedException();
        }

        public void SetMeetingPurpose(Meeting meeting, Contact contact, MeetingPurpose purpose)
        {
            throw new NotImplementedException();
        }

        public void SetMeetingSatisfaction(Meeting meeting, Contact contact, MeetingSatisfaction satisfaction)
        {
            throw new NotImplementedException();
        }
    }
}
