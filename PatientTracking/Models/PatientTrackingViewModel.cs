
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class PatientTrackingViewModel
    {
        public PatientTrackingViewModel()
        {

        }
        public List<Doctor> Doctors { get; set; }
        public List<Patient> Patients { get; set; }
        public List<Pharmacy> Pharmacies { get; set; }

        public Patient patient { get; set; }


    }
}
