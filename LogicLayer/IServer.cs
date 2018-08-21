using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IServer
    {
        /// <summary>
        /// Find the next meeting of the contact and returns it 
        /// </summary>
        /// <param name="contact">the contact name</param>
        /// <returns>the next meeting</returns>
        Meeting FindNextMeeting(Contact contact);

        /// <summary>
        /// Find the last 2 meeting between 2 contacts
        /// </summary>
        /// <param name="a">contact number 1</param>
        /// <param name="b">contact number 2</param>
        /// <returns>the last meeting</returns>
        List<Meeting> FindLastMeetingWithContact(Contact a, Contact b);

        /// <summary>
        /// returns the contact object by name
        /// </summary>
        /// <param name="name">the contact name</param>
        /// <returns>the contact</returns>
        Contact GetContactByName(string name);

        /// <summary>
        /// Returns tips about a countact for a specific meeting.
        /// </summary>
        /// <param name="meeting">the meeting</param>
        /// <param name="contact">the contact to get tips about him</param>
        /// <param name="isCloseToTheMeeting">represent if the tips can be very close to the meeting</param>
        /// <returns>list of tips</returns>
        List<Tip> GetTipsAboutPersonForMeeting(Meeting meeting, Contact contact, bool isCloseToTheMeeting);

        /// <summary>
        /// set the meeting purpose
        /// </summary>
        /// <param name="meeting">the meeting</param>
        /// <param name="contact">the contact that set the purpose</param>
        /// <param name="purpose">the meeting purpose</param>
        void SetMeetingPurpose(Meeting meeting, Contact contact, MeetingPurpose purpose);

        /// <summary>
        /// Set the rate for a tip, for a specific contact in a specific meeting
        /// </summary>
        /// <param name="meeting">the meeting</param>
        /// <param name="contact">the contact that the tip is about him</param>
        /// <param name="tipe">the tip</param>
        /// <param name="WasGood"></param>
        void RateTip(Meeting meeting, Contact contact, Tip tip, bool WasGood);

        /// <summary>
        /// rate the meeting in general
        /// </summary>
        /// <param name="meeting">the meeting</param>
        /// <param name="contact">the contact that gives the rate for the meeting</param>
        /// <param name="satisfaction">the rate for the meeting</param>
        void SetMeetingSatisfaction(Meeting meeting, Contact contact, MeetingSatisfaction satisfaction);

        /// <summary>
        /// Get the tips that some contact gots (used after the meeting)
        /// </summary>
        /// <param name="meeting">the meeting</param>
        /// <param name="contact">the contact</param>
        /// <returns>list of tips</returns>
        List<Tip> GetGivenTips(Meeting meeting, Contact contact);
    }
}
