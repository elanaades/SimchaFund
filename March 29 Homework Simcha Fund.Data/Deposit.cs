using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace March_29_Homework_Simcha_Fund.Data
{
    public class Deposit
    {
        public int ContributorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DepositDate { get; set; }
        public decimal DepositAmount { get; set; }
    }
}
