namespace SportPlannerIngestion.DataLayer.Models
{
    public class Address : BaseEntity
    {
        public string FullAddress { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
