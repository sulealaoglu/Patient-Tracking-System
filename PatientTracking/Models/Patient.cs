namespace WebApplication1.Models
{
    public class Patient
    {
        public Patient()
        {

        }
        public string bdate{ get; set; }
        public Doctor dr { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string treatment{ get; set; }
        public string drug { get; set; }
        public string userName { get; set; }
        public int id { get; set; }

        public List<DrugPharmacyViewModel> pharmacies { get; set; }


    }
}
