using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesLayer;

namespace DataBase
{
    public interface IDB
    {
        List<Meeting> FindLastMeeting(Contact a, Contact b);
        Meeting FindNextMeeting(Contact contact);
        Contact GetContactByName(string name);
        List<Tip> GetTipsAboutPersonForMeeting(Meeting meeting, Contact contact, bool isCloseToTheMeeting);
    }
}
