using EntitiesLayer;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = ServerMock.GetInstance();
            Contact curContact = server.GetContactByName("shir esh");
            string name = curContact.Name;
            string image = curContact.ImagePath;
            string linkedin = curContact.LinkedinPath;

            Meeting meeting = server.FindNextMeeting(curContact);
            string title = meeting.Title;
            Console.Read();
        }
    }
}
