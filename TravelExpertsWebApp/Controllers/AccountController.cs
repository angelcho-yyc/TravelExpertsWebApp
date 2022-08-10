using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelExpertsData;

namespace TravelExpertsWebApp.Controllers
{
    public class AccountController : Controller
    {
        // register page
        public IActionResult Register()
        {
            return View();
        }

        // post register form data of new customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Customer newCust)
        {
            // check if there is existing username
            string msg = CustomerManager.CheckUserIdExists(newCust.UserId);
            if (!string.IsNullOrEmpty(msg))
            {
                ModelState.AddModelError(nameof(newCust.UserId), msg); // add model error and display error message
                return View(newCust);
            }

            if (ModelState.IsValid) // if form data is ok
            {
                try
                {
                    CustomerManager.AddCustomer(newCust); // add new customer to the database
                    TempData["Message"] = "Registration completed.";
                    return RedirectToAction("Login"); // redirect to login page
                }
                catch
                {
                    TempData["IsError"] = true;
                    TempData["Message"] = "Error when attempting to register. Try again later";
                    return View(newCust); //stay on the same view
                }
            }
            else
            {
                return View(newCust); // stay on the same view
            }
        }

        public IActionResult Login(string returnUrl = "")
        {
            if (returnUrl != null)
            {
                TempData["ReturnUrl"] = returnUrl;  // keep the returnUrl if any
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Customer customer)
        {
            Customer cust = CustomerManager.Authenticate(customer.UserId, customer.Password);
            if (cust == null) // authentication failed
            {
                TempData["IsError"] = true;
                TempData["Message"] = "Authentication failed";
                return View(); // stay on login page
            }

            // authentication passed

            HttpContext.Session.SetInt32("CurrentCustomer", cust.CustomerId); // store customer Id in session

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, cust.UserId),
                new Claim(ClaimTypes.GivenName, cust.CustFirstName),
                new Claim(ClaimTypes.Surname, cust.CustLastName),
                new Claim(ClaimTypes.StreetAddress, cust.CustAddress),
                new Claim(ClaimTypes.HomePhone, cust.CustHomePhone),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (string.IsNullOrEmpty(TempData["ReturnUrl"].ToString()))
            {
                return RedirectToAction("Index", "Home"); // if no page requested, go to Home/Index
            }
            else
            {
                return Redirect(TempData["ReturnUrl"].ToString()); // go to previously requested URL
            }
        }

        public async Task<IActionResult> LogoutAsync()
        {
            int? custId = HttpContext.Session.GetInt32("CurrentCustomer");
            if (custId != null)
                HttpContext.Session.Remove("CurrentCustomer");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); // go to Home/Index after signing out
        }
                
    }
}
