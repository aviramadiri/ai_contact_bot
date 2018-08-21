using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLayer
{
    public class Contact
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public CommunicationStyle[] Styles { get; set; }
        public string ImagePath { get; set; }
        public string LinkedinPath { get; set; }
        public Dictionary<Tip, int> TipsRate { get; set; } 
    }
}
