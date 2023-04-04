using March_29_Homework_Simcha_Fund.Data;
using March_29_Homework_Simcha_Fund.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace March_29_Homework_Simcha_Fund.Web.Controllers
{
    public class ContributorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Contributor contributor, decimal initialdeposit)
        {
            var db = new DatabaseManager();
            int id = db.AddContributor(contributor);
            var deposit = new Deposit
            {
                ContributorId = id,
                FirstName = contributor.FirstName,
                LastName = contributor.LastName,
                DepositDate = contributor.DateCreated,
                DepositAmount = initialdeposit,
            };
            db.AddDeposit(deposit);

            TempData["contributor-message"] = $"New Contributor Added!";

            return Redirect("/home/contributors");
        }

        [HttpPost]
        public IActionResult Deposit(Deposit deposit)
        {
            var db = new DatabaseManager();

            db.AddDeposit(deposit);
            TempData["contributor-message"] = $"Deposit Successfully Recorded!";
            return Redirect("/home/contributors");
        }

        public IActionResult History(int contribid)
        {
            var db = new DatabaseManager();
            List<Transactions> transactions = db.GetDepositsByContributor(contribid);
            transactions.AddRange(db.GetContributionsByContributor(contribid));
            List<Transactions> sortedTransactions = transactions.OrderBy(t => t.Date).ToList();

            ContributorViewModel vm = new ContributorViewModel
            {
                Transactions = sortedTransactions,
                Balance = db.GetBalance(contribid),
                Name = db.GetNameById(contribid),
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(Contributor c)
        {
            var db = new DatabaseManager();
            db.EditContributor(c);
            TempData["contributor-message"] = $"Contributor Updated Successfully!";

            return Redirect("/home/contributors");
        }

    }
}
