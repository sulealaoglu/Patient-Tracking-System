namespace WebApplication1.Models
{
    public class AddAppointmentViewModel
    {
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentTime { get; set; }
    }
}
