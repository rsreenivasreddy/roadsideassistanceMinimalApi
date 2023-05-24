using System.Reflection.Metadata;

namespace RoadsideAssistance.Api.Data.DomianEntities
{
    public class Assistant :DomainEntity
    {
        public Assistant()
        {
            
        }
        public Assistant(string description,int currentGeoLocationId)
        {
            Description = description;
            CurrentGeoLocationId = currentGeoLocationId;
            IsReserved = false;  // Intially when we create assistant , he is not reserved for any csutomers.
        }
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public int CurrentGeoLocationId { get; set; }
        public bool IsReserved { get; set; }

        public GeoLocation GeoLocation { get; set; } = null!;

        public ICollection<CustomerAssistant> CustomerAssistants { get; set; } = null!;

        public bool Reserve(int locationId)
        {
            if (!IsReserved)
            {
                IsReserved = true;
                CurrentGeoLocationId = locationId;
                return true;
            }
            return false; // As Assitant Already Reserved for Some Other Customer this shouldn't happen Ideally , but an additional check.
        }
        public bool Release()
        {
            if (IsReserved)
            {
                IsReserved = false;
                return true;
            }
            return false; // As Assitant Already Released this shouldn't happen Ideally , but an additional check.
        }
    }
}
