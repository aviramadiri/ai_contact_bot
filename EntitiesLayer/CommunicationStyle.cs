using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLayer
{
    public class CommunicationStyle
    {
        public double Rate { get; set; } // should be between 0 to 9
        public Style CommStyle { get; set; }
    }

    public enum Style
    {
        // TODO: what should be here?
        MissionDriven, supporter, analyst, colorful
    }
}
