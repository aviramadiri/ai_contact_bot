using EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class TipMatrix
    {
        public List<Tip> Tips { get; set; }

        public static TipMatrix BuildMatrix()
        {
            var tips = new List<Tip>(); //TODO: add tips
            return new TipMatrix()
            {
                Tips = tips
            };
        }

        internal List<Tip> GetTipsByPurposeAndSatisfaction(MeetingPurpose purpose, bool isCloseToTheMeeting)
        {
            List<Tip> tips = Tips.Where(t => t.IsImmediately == isCloseToTheMeeting && t.Purposes.Contains(purpose)).ToList();
            return tips;
        }
    }
}
