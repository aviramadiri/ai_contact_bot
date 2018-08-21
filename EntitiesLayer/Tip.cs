using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLayer
{
    public class Tip
    {
        public string Content { get; set; }
        public List<CommunicationStyle> Styles { get; set; }
        public List<MeetingPurpose> Purposes { get; set; }
        public bool IsImmediately { get; set; }

        public double CalculateRate(List<CommunicationStyle> styles)
        {
            double rate = 0;
            foreach (CommunicationStyle cs in Styles)
            {
                Style style = cs.CommStyle;
                double r = cs.Rate;
                CommunicationStyle c = styles.Where(s => s.CommStyle == style).FirstOrDefault();
                rate += r * c.Rate;
            }

            return rate;
        }
    }
}
