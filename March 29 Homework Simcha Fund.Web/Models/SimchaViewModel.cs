using March_29_Homework_Simcha_Fund.Data;
using March_29_Homework_Simcha_Fund.Web;

namespace March_29_Homework_Simcha_Fund.Web.Models
{
    public class SimchaViewModel
    {
        public List<Simcha> Simchas { get; set; }
        public int SimchaId { get; set; }
        public string SimchaName { get; set; }
        public List<Contributor> Contributors {get;set;}
        public List<Contributions> Contributions { get; set; }
        public string SimchaMessage { get; set; }
    }
}
