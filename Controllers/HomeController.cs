using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BankAccounts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BankAccounts.Controllers
{
    public class HomeController : Controller
    {
        private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }

        private bool isLoggedIn
        {
            get
            {
                return uid != null;
            }
        }
        private BankAccountsContext db;
        public HomeController(BankAccountsContext context)
        {
            db = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            // Refactor: Return redirect if user is session and tries to access login/registration page
            if (isLoggedIn)
            {
                return RedirectToAction("Success", uid);
            }

            return View("Index");
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(db.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");

                    // return View("Index");
                }
            }
            if (ModelState.IsValid == false)
            {
                // So error messages will be displayed.
                return View("Index");
            }

            PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

                db.Add(newUser);
                db.SaveChanges();

                // Refactor: Instead of saving the entire User into session, just save UserId and User name from newUser

                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                HttpContext.Session.SetString("FirstName", newUser.FirstName);
                HttpContext.Session.SetString("LastName", newUser.LastName);

                return RedirectToAction("Success", uid);

        }

        [HttpGet("account/{userId}")]
        public IActionResult Success(int userId)
        {
            User user = db.Users
            .Include(user => user.AllTransactions).FirstOrDefault(u => u.UserId == userId);

            if(user != null)
            {   
                ViewBag.User = db.Users
                    .Include(user => user.AllTransactions).FirstOrDefault(u => u.UserId == userId);
                return View("UserDashboard");
            } else {
                return RedirectToAction("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = db.Users.FirstOrDefault(u => u.Email == userSubmission.loginEmail);

                if(userInDb == null)
                {
                    ModelState.AddModelError("loginEmail", "Email doesn't exist");
                    return View("Index");
                }

                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();

                PasswordVerificationResult result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.loginPassword);

                // Result can be compared to 0 for failure
                if(result == 0)
                {
                    ModelState.AddModelError("loginEmail", "Invalid email/password");

                    return View("Index");
                }
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                HttpContext.Session.SetString("FirstName", userInDb.FirstName);
                HttpContext.Session.SetString("LastName", userInDb.LastName);

                return RedirectToAction("Success");
            } else {
                return View("Index");
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
