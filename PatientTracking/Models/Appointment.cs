namespace WebApplication1.Models
{
    public class Appointment
    {
        public Guid AppointmentID { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentTime { get; set;}


    }
}
