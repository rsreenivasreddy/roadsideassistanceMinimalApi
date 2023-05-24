namespace RoadsideAssistance.Api.Data.DomianEntities
{
    public class GeoLocation:DomainEntity
    {
        public GeoLocation()
        {
            Assistants = new List<Assistant>();
            CustomerAssistants = new List<CustomerAssistant>();
        }
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
      
        public string Address { get; set; } = null!;

        public ICollection<Assistant> Assistants { get; set; } = null!;
        public ICollection<CustomerAssistant> CustomerAssistants { get; set; } = null!;
    }
}
