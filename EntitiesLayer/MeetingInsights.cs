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
        public string Purpose { get; set; }
        public string Satisfaction {get;set;}

        public MeetingInsights()
        {
            Tips = new Dictionary<Tip, int>();
            Purpose = MeetingPurpose.General;
            Satisfaction = MeetingSatisfaction.None;
        }
    }

    public static class MeetingPurpose
    {
        public static string General = "General";
        public static string convince = "To convince";
        public static string Motivate = "To motivate";
        public static string Prize = "To prize";
        public static string Critisize = "To Critisize";

        public static List<string> GetPurposes()
        {
            return new List<string>()
            {
                convince, Motivate, Prize, Critisize
            };
        }
    }

    public static class MeetingSatisfaction
    {
        // TODO: what should be here?

        public static string None = "None";
        public static string VeryGood = "Very Good";
        public static string Good = "Good";
        public static string NotGood = "Not Good";
        public static string Bad = "Bad";

        public static List<string> GetMeetingSatisfactions()
        {
            return new List<string>()
            {
                VeryGood, Good, NotGood, Bad
            };
        }
    }
}
