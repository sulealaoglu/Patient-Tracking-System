using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.Models;
using WebApplication1.Repositories;
using Database = WebApplication1.Repositories.Database;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
        
        public IActionResult ShowData(int id)
        {
            List<Patient> patients = Database.GetAllPatients();


            return View(patients);
        }
        public IActionResult Log()
        {
            List<LogViewModel> logs = Database.getLogs();

            return View(logs);
        }
        public IActionResult Delete()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
          int res=  Database.Delete_Patient(id);
            if(res==1)
                return RedirectToAction("ShowData", "Admin");
           
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(DateTime bdate,string name,string surname,string treatment,string drug,int  drug_id)
        { int did = Int32.Parse(TempData["Veri"].ToString());
            Doctor dr=Database.findDoctor(did);
            int res= Database.Create( name, surname, bdate, treatment,  drug_id,dr );
            if (res==1)
            {
                return RedirectToAction("Admin", "Admin");
            }
            
            return View();
        }
    }
}
