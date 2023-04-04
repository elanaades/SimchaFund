using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace March_29_Homework_Simcha_Fund.Data
{
    public class Contributor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool AlwaysInclude { get; set; }
        public string CellNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Balance { get; set; }
    }
}
