using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesLayer;

namespace LogicLayer
{
    public class MockServer
    {
        private Contact aviram;
        private Contact shir;
        private List<Meeting> prevMeetings;
        private Meeting currMeeting;
        private List<Tip> tips;

        public MockServer()
        {
            aviram = CreateContact("aviram.adiri@hotmail.com", "Aviram Adiri", "https://www.linkedin.com/in/aviram-adiri-19bbb5108/", "https://media.licdn.com/dms/image/C5603AQE0qY4fwL1RvA/profile-displayphoto-shrink_200_200/0?e=1540425600&v=beta&t=FyhjvnR8D3k_2WAfVULRrEFTP9KlaFUk8-WNvJUKyNI", analyst: 70, colorful: 30);
            shir = CreateContact("shir@hotmail.com", "Shir Ash", "https://www.linkedin.com/in/shir-esh-aa5981165/", "data:image/jpeg;base64,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", missionDriven: 70, supporter: 30);
            currMeeting = new Meeting(1, "Design review for AI-Contact", new DateTime(2018, 9, 1, 10, 0, 0), new List<Contact>() { shir, aviram });
            var m1 = new Meeting(2, "Going over technical tools", new DateTime(2018, 8, 20, 13, 0, 0), new List<Contact>() { shir, aviram });
            var m2 = new Meeting(3, "AI-Contact spec - going over requirments", new DateTime(2018, 8, 10, 11, 0, 0), new List<Contact>() { shir, aviram });
            prevMeetings = new List<Meeting>() { m1, m2 };
            var t1 = new Tip() { Content = "this is tip number one" };
            var t2 = new Tip() { Content = "this is tip number two" };
            tips = new List<Tip>() { t1, t2 };
        }

        private Contact CreateContact(string email, string name, string linkedinPath, string imagePath, int missionDriven = 0, int supporter = 0, int analyst = 0, int colorful = 0)
        {
            return new Contact()
            {
                Email = email,
                Name = name,
                Styles = new CommunicationStyle[]
                {
                    new CommunicationStyle() { CommStyle = Style.MissionDriven, Rate = missionDriven},
                    new CommunicationStyle() { CommStyle = Style.supporter, Rate = supporter},
                    new CommunicationStyle() { CommStyle = Style.analyst, Rate = analyst},
                    new CommunicationStyle() { CommStyle = Style.colorful, Rate = colorful}
                },
                LinkedinPath = linkedinPath,
                ImagePath = imagePath,
                TipsRate = new Dictionary<Tip, int>()
            };
        }

        public List<Meeting> FindLastMeetingWithContact(Contact a, Contact b)
        {
            return prevMeetings;
        }

        public Meeting FindNextMeeting(Contact contact)
        {
            return currMeeting;
        }

        public Contact GetContactByName(string name)
        {
            return aviram;
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
        }

        public void SetMeetingPurpose(Meeting meeting, Contact contact, string purpose)
        {
        }

        public void SetMeetingSatisfaction(Meeting meeting, Contact contact, string satisfaction)
        {
        }
    }
}
