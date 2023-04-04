using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace March_29_Homework_Simcha_Fund.Data
{
    public class Contributions
    {
        public int ContributorId { get; set; }
        public int SimchaId { get; set; }
        public decimal ContributionAmount { get; set; }
        public bool Include { get; set; }
    }
}
