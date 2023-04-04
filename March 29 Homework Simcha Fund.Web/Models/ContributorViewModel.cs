using March_29_Homework_Simcha_Fund.Data;

namespace March_29_Homework_Simcha_Fund.Web.Models
{
    public class ContributorViewModel
    {
        public List<Contributor> Contributors { get; set; }
        public List<Transactions> Transactions { get; set; }
        public decimal Balance { get; set; }
        public string Name { get; set; }
        public decimal TotalContributions { get; set; }
        public string ContributorMessage { get; set; }
    }
}
