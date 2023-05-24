using RoadsideAssistance.Api.Data;
using RoadsideAssistance.Api.Data.DomianEntities;
using RoadsideAssistance.Api.Data.Enums;
using RoadsideAssistance.Api.Models.Request;
using RoadsideAssistance.Api.Models.Response;
using System.Linq.Expressions;

namespace RoadsideAssistance.Api.Services
{
    public class CustomerAssistantService : ICustomerAssistantService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Assistant> _assistantRepository;
        private readonly IRepository<GeoLocation> _geoLocationRepository;
        private readonly IRoadsideAssistanceService _roadsideAssistanceService;

        public CustomerAssistantService(IRepository<Customer> customerRepository, 
            IRepository<Assistant> assistantRepository,
            IRepository<GeoLocation> geoLocationRepository,
            IRoadsideAssistanceService roadsideAssistanceService)
        {
            _customerRepository = customerRepository;
            _assistantRepository = assistantRepository;
            _geoLocationRepository = geoLocationRepository;
            _roadsideAssistanceService = roadsideAssistanceService;
        }
        public async Task<IResult> FindNearestAssistants(GeoLocationRequest geoLocationRequest,int limit, CancellationToken cancellationToken = default)
        {
            if(limit <0 || limit > 50)
            {
                return Results.BadRequest("Sorry!  Limit is out of Accepted range (1 to 50)");
            }
            var geoLocation = GetGeoLocation(geoLocationRequest);
            if (geoLocation == null)
            {
                return Results.BadRequest("Sorry!  Requested Location out of Service Provider Area"); 
            }

            var nearestAvailableAssistants = await _roadsideAssistanceService.FindNearestAssistants(geoLocation, limit);
            var response = nearestAvailableAssistants.Select(a => new AssistantResponse(a)).ToList();
            return Results.Ok(response);
        }

        public async Task<IResult> ReserveAssistant(int customerId, GeoLocationRequest geoLocationRequest, CancellationToken cancellationToken = default)
        {
            var customer = await _customerRepository.FindAsync(customerId, cancellationToken);
            if (customer == null)
            {
                return Results.NotFound("Customer Not Found");
            }
            var geoLocation = GetGeoLocation(geoLocationRequest);
            if (geoLocation == null)
            {
                return Results.BadRequest("Sorry!  Requested Location out of Service Provider Area");
            }

            (Assistant? Assistant, ErrorType? ErrorType) reservation = await _roadsideAssistanceService.ReserveAssistant(customer, geoLocation);
            if (reservation.Assistant == null)
            {
                if (reservation.ErrorType == ErrorType.ConflictError)
                    return Results.Conflict("Sorry! An Assistant altready serving the Customer, Can't reserve more than one Assistant");
                else if (reservation.ErrorType == ErrorType.NotFoundError)
                    return Results.NotFound("Sorry! Unable to Find Service Assistant for the Requested Location");
                else
                    return Results.BadRequest("Sorry! Unable to Provide Service Assistant now");
            }

            return Results.Ok(new AssistantResponse(reservation.Assistant));
        }
        public async Task<IResult> ReleaseAssistant(int customerId, int assistantId, CancellationToken cancellationToken = default)
        {
            var customer = await _customerRepository.FindAsync(customerId, cancellationToken);
            if (customer == null)
            {
                return Results.NotFound("Customer Not Found");
            }
            var assistant = await _assistantRepository.FindAsync(assistantId, cancellationToken);
            if (assistant == null)
            {
                return Results.NotFound("Assistant Not Found");
            }
            var releaseReservation = await _roadsideAssistanceService.ReleaseAssistant(customer, assistant);
            if(!releaseReservation.IsSuccess)
            {
                if(releaseReservation.ErrorType == ErrorType.NotFoundError)
                    return Results.BadRequest("Sorry! Unable to find Active(Inprogress) Reserved Assistant Servicing this Customer to Release");
                else
                    return Results.BadRequest("Sorry! Unable to Release an Assistant at this time");
            }
            return Results.Ok();
        }

       

        public async Task<IResult> UpdateAssistantLocation(int assistantId, GeoLocationRequest geoLocationRequest, CancellationToken cancellationToken = default)
        {

            var geoLocation =  GetGeoLocation(geoLocationRequest);
            if (geoLocation == null)
            {
                return Results.BadRequest("Sorry! Unable to Provide Service for the Requested Location");
            }
            var assistant = await _assistantRepository.FindAsync(assistantId, cancellationToken);
            if(assistant == null)
            {
                return Results.NotFound();
            }
            await _roadsideAssistanceService.UpdateAssistantLocation(assistant, geoLocation);
            return Results.Ok(new AssistantResponse(assistant));
        }

        private GeoLocation? GetGeoLocation(GeoLocationRequest geoLocationRequest)
        {
            Expression<Func<GeoLocation, bool>> geoLocationExpression = g => g.Latitude == geoLocationRequest.Latitude && g.Longitude == geoLocationRequest.Longitude;
            return _geoLocationRepository.Entity.FirstOrDefault(geoLocationExpression);
        }

    }
}
