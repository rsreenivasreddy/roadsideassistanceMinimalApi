using RoadsideAssistance.Api.Data.Enums;

namespace RoadsideAssistance.Api.Data.DomianEntities
{
    public class CustomerAssistant : DomainEntity
    {
        public CustomerAssistant()
        {
            
        }
        public CustomerAssistant(int customerId)
        {
            CustomerId = customerId;
        }
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AssistantId { get; set; }
        public int GeoLocationId { get; set; }
        public DateTimeOffset ServiceStartDate { get; set; }
        public DateTimeOffset? ServiceCompleteDate { get; set; }
        public string Status { get; set; } = null!;

        public Assistant Assistant { get; set; } = null!;

        public Customer Customer { get; set; } = null!;
        public GeoLocation GeoLocation { get; set; } = null!;

        public bool ReserveAssistant(Assistant assistant,int geoLocationId)
        {
            if (assistant == null)
            {
                throw new ArgumentNullException("Unable to Reserve, Assistant parameter is null");
            }
            bool canReserve = assistant.Reserve(geoLocationId);
            if (canReserve)
            {
                Assistant = assistant;
                GeoLocationId = geoLocationId;
                ServiceStartDate = DateTimeOffset.Now;
                Status = ServiceStatus.InProgress.ToString();
                return true;
            }
            return false;
        }

        public bool ReleaseAssistant()
        {
            if (Assistant == null)
            {
                throw new ArgumentNullException("Unable to Release, Assistant parameter is null");
            }

            if (Status != ServiceStatus.InProgress.ToString() || !Assistant.IsReserved)
            {
                throw new Exception("Unable to Release Assistance, Assistant is already released or Status update is missing");
            }
            bool released = Assistant.Release();
            if (released)
            {
                Status = ServiceStatus.Completed.ToString();
                ServiceCompleteDate = DateTimeOffset.Now;
                return true;
            }

            return false;
        }
    }
}
