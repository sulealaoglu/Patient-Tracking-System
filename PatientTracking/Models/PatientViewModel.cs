namespace WebApplication1.Models
{
    public class PatientViewModel

    {
        public string bdate { get; set; }
        public String d_name { get; set; }
        public String d_surname { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string treatment { get; set; }
        public string drug { get; set; }
        public int id { get; set; }

        public List<DrugPharmacyViewModel> pharmacies { get; set; }

    }
}
