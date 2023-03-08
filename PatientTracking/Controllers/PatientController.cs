using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Pharmacies()
        { List<Pharmacy> ph=Database.GetAllPharmacies();
            return View(ph);
        }
        public IActionResult Appointment()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Appointment(int pid,int did,DateTime date,DateTime time)
        { AddAppointmentViewModel appointment=new AddAppointmentViewModel { 
        PatientID= pid,DoctorID= did,AppointmentDate= date,AppointmentTime  = time  
        
        };
            Database.CreateAppointment( appointment);
            return View();
        }
        public IActionResult Patient()
        {   
            List< Patient> p = Database.GetAllPatients();
            Patient pt;
            int pt_id = Int32.Parse(TempData["Veri"].ToString());
            pt = Database.findPatient(pt_id);
           
            return View(pt);
        }
        public IActionResult Treatment()
        {
            PatientTrackingViewModel patient_tracking = new PatientTrackingViewModel()
            {
                Doctors = Database.GetAllDoctors(),
                Patients = Database.GetAllPatients(),
                Pharmacies= Database.GetAllPharmacies()
                
            };
                return View(patient_tracking);
        }
        
    }
}