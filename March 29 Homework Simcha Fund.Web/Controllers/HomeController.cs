using March_29_Homework_Simcha_Fund.Data;
using March_29_Homework_Simcha_Fund.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace March_29_Homework_Simcha_Fund.Web.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            var db = new DatabaseManager();
            SimchaViewModel vm = new SimchaViewModel
            {
                Simchas = db.GetSimchas()
            };

            if (TempData["simcha-message"] != null)
            {
                vm.SimchaMessage = (string)TempData["simcha-message"];
            }

            return View(vm);
        }

        public IActionResult Contributors()
        {
            var db = new DatabaseManager();
            ContributorViewModel vm = new ContributorViewModel
            {
                Contributors = db.GetContributors(),
                TotalContributions = db.GetTotalContributionsForAll()
            };

            if (TempData["contributor-message"] != null)
            {
                vm.ContributorMessage = (string)TempData["contributor-message"];
            }

            return View(vm);
        }


    }
}