using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLayer
{
    public class MeetingInsights
    {
        public Dictionary<Tip, int> Tips { get; set; }
        public MeetingPurpose Purpose { get; set; }
        public MeetingSatisfaction Satisfaction {get;set;}

        public MeetingInsights()
        {
            Tips = new Dictionary<Tip, int>();
            Purpose = MeetingPurpose.general;
            Satisfaction = MeetingSatisfaction.none;
        }
    }

    public enum MeetingPurpose
    {
        // TODO: what should be here?
        general, p2, p3, p4
    }

    public enum MeetingSatisfaction
    {
        // TODO: what should be here?
        none, VeryGood, Good, Ok, NotGood, Bad
    }
}
