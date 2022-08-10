using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelExpertsData;

namespace TravelExpertsWebApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {       
        public IActionResult Profile()
        {
            int? custId = HttpContext.Session.GetInt32("CurrentCustomer"); // get customer Id from session
            if (custId != null)
            {
                try
                {
                    Customer customer = CustomerManager.GetCustomer((int)custId);
                    return View(customer);   // return view with information of signed in customer
                }
                catch
                {
                    TempData["IsError"] = true;
                    TempData["Message"] = "Error when attempting to retrieve your profile. Try again later";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(Customer newData)
        {
            if (ModelState.IsValid)
            {
                Customer customer = CustomerManager.UpdateInfo(newData);
                TempData["Message"] = "Information updated";
                return View(customer);   // return view with information of signed in customer
            }
            else
            {
                return View(newData);  // stay on the same view
            }               
        }
    }
}
