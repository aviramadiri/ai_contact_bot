using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class MockDB
    {
        Dictionary<string, Contact> Contacts { get; set; }
        Dictionary<int, Meeting> Meetings { get; set; }
        Dictionary<string, Tip> Tips { get; set; }
    }
}
