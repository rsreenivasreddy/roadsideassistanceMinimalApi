using RoadsideAssistance.Api.Data.DomianEntities;
using RoadsideAssistance.Api.Models.Request;

namespace RoadsideAssistance.Api.Services
{
    public interface ICustomerAssistantService
    {
        Task<IResult> UpdateAssistantLocation(int assistantId, GeoLocationRequest geoLocationRequest, CancellationToken cancellationToken = default);
        Task<IResult> FindNearestAssistants(GeoLocationRequest geoLocationRequest, int limit, CancellationToken cancellationToken = default);
        Task<IResult> ReserveAssistant(int customerId,GeoLocationRequest geoLocationRequest, CancellationToken cancellationToken = default);
        Task<IResult> ReleaseAssistant(int customerId, int assistantId,  CancellationToken cancellationToken = default);
       
    }
}
