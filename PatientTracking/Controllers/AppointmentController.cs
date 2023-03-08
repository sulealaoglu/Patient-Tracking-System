using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Repositories;
namespace WebApplication1.Controllers
{
    public class AppointmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
  [HttpPost]
        public IActionResult Add(int PatientId, int DoctorID, DateTime AppointmentDate, DateTime AppointmentTime)
        {
           
            Console.WriteLine();
            return View();
        }
        
        public IActionResult Add()
        {       
            return View();
        }

        public IActionResult a()
        {
            return View();
        }

    }
}
