using Microsoft.AspNetCore.Mvc;
using Virtual_Wallet.Models.ViewModels;

namespace Virtual_Wallet.Controllers.MVC
{
    public class TransferController : Controller
    {
        [HttpGet]
        public IActionResult Transfer()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult TransferFunds(TransferViewModel model)
        //{
        //    // Logic for handling MVC form submission
        //    return Ok(model);
        //}
    }
}
