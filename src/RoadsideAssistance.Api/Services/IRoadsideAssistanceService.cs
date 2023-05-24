using RoadsideAssistance.Api.Data.DomianEntities;
using RoadsideAssistance.Api.Data.Enums;

namespace RoadsideAssistance.Api.Services
{
    public interface IRoadsideAssistanceService
    {
        /**
        * This method is used to update the location of the roadside assistance service provider.
        *
        * @param assistant represents the roadside assistance service provider
        * @param assistantLocation represents the location of the roadside assistant
        */
        Task<Assistant> UpdateAssistantLocation(Assistant assistant, GeoLocation assistantLocation);
        /**
        * This method returns a collection of roadside assistants ordered by their distance from the input geo location.
        *
        * @param geolocation - geolocation from which to search for assistants
        * @param limit - the number of assistants to return
        * @return a sorted collection of assistants ordered ascending by distance from geoLocation
        */
        Task<IEnumerable<Assistant>> FindNearestAssistants(GeoLocation geolocation, int limit);
        /**
        * This method reserves an assistant for a Geico customer that is stranded on the roadside due to a disabled vehicle.
        *
        * @param customer - Represents a Geico customer
        * @param customerLocation - Location of the customer
        * @return The Assistant that is on their way to help
        */
        Task<(Assistant? Assistant, ErrorType? ErrorType)> ReserveAssistant(Customer customer, GeoLocation customerLocation);
        /**
        * This method releases an assistant either after they have completed work, or the customer no longer needs help.
        *
        * @param customer - Represents a Geico customer
        * @param assistant - An assistant that was previously reserved by the customer
        */
        Task<(bool IsSuccess, ErrorType? ErrorType)> ReleaseAssistant(Customer customer, Assistant assistant);
    }
}
