using March_29_Homework_Simcha_Fund.Data;
using March_29_Homework_Simcha_Fund.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Transactions;

namespace March_29_Homework_Simcha_Fund.Web.Controllers
{
    public class SimchaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewSimcha(Simcha simcha)
        {
            var db = new DatabaseManager();
            db.AddSimcha(simcha);
           TempData["simcha-message"] = $"Simcha Has Been Added Successfully!";

            return Redirect("/home/index");
        }

        public IActionResult Contributions(int simchaId)
        {
            var db = new DatabaseManager();
            SimchaViewModel vm = new SimchaViewModel
            {
                Contributors = db.GetContributors(),
                SimchaId = simchaId,
                SimchaName = db.GetSimchaName(simchaId),
                Contributions = db.GetContributionsBySimcha(simchaId),
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult UpdateContributions(List<Contributions> contributions, int simchaId)
        {
            var db = new DatabaseManager();
            List<Contributions> addContributions = contributions.Where(c => c.Include).ToList();
            db.DeleteContributions(simchaId);
            db.UpdateContributions(addContributions, simchaId);

            TempData["simcha-message"] = $"Simcha Updated Successfully!";


            return Redirect("/home/index");
        }
    }
}
