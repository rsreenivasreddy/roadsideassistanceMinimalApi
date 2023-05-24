namespace RoadsideAssistance.Api.Data.DomianEntities
{
    public class Customer : DomainEntity
    {
        public Customer(string name,string vehicleVINNumber,string vehicleMakeModel)
        {
            Name = name;
            VehicleVINNumber = vehicleVINNumber;
            VehicleMakeModel = vehicleMakeModel;
            CustomerAssistants = new List<CustomerAssistant>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string VehicleVINNumber { get; set; } = null!;
        public string VehicleMakeModel { get; set;} = null!;

        public ICollection<CustomerAssistant> CustomerAssistants { get; set; } = null!;

        //public bool ReserveAssistant(Assistant assistant,int geoLocationId)
        //{

        //    var isReserved = assistant.ReserveForLocation(geoLocationId);
        //    if (isReserved)
        //    {
        //        CustomerAssistants.Add(new CustomerAssistant())
        //        return true;
        //    }
        //    var customerAssistnt = new CustomerAssistant(
        //}

    }
}
