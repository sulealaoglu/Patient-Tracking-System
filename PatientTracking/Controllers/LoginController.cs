using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class LogInController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TakeData()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TakeData(string mail, string password)
        {
            var user_t = Database.findUser_P(mail, password);
            if (user_t == null)
            { return View(); }
            Type t = user_t.GetType();
            if (t == typeof(Patient))
            {
                Patient pt = (Patient)user_t;
                TempData["Veri"] = pt.id;
                TempData.Keep();
                Database.addLog(mail);
                return RedirectToAction("Patient", "Patient");
            }
            else if (t == typeof(Doctor))
            {
                Doctor dr = (Doctor)user_t;
                TempData["Veri"] = dr.id;
                TempData.Keep();
                Database.addLog(mail);
                return RedirectToAction("Admin", "Admin");
            }

            else return View();

        }
    }
   
    }
