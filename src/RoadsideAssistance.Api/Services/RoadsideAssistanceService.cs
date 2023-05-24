using Microsoft.Data.SqlClient;
using RoadsideAssistance.Api.Data;
using RoadsideAssistance.Api.Data.DomianEntities;
using RoadsideAssistance.Api.Data.Enums;
using RoadsideAssistance.Api.Models.Request;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace RoadsideAssistance.Api.Services
{
    public class RoadsideAssistanceService : IRoadsideAssistanceService
    {
        private readonly IRepository<Assistant> _assistantRepository;
        private readonly IRepository<CustomerAssistant> _customerAssistantRepository;
        public RoadsideAssistanceService(IRepository<Assistant> assistantRepository,
            IRepository<CustomerAssistant> customerAssistantRepository)
        {
            _assistantRepository = assistantRepository;
            _customerAssistantRepository = customerAssistantRepository;
        }

        /// <summary>
        /// Returns Nearest  Available ( NOT Resserved ) Assistants.
        /// </summary>
        /// <param name="geolocation"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Assistant>> FindNearestAssistants(GeoLocation geolocation, int limit)
        {
            SqlParameter[] sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@limit", limit);
            sqlParameters[1] = new SqlParameter("@latitude", geolocation.Latitude);
            sqlParameters[2] = new SqlParameter("@longitude", geolocation.Longitude);

            string sqlQuery = @"Select Top(@limit) Id,[Description],CurrentGeoLocationId,IsReserved from( 
                            SELECT a.*, (3959 * acos(cos(radians(@latitude)) * cos(radians(l.latitude)) * cos(radians(l.longitude) - radians(@longitude)) + sin(radians(@latitude)) * sin(radians(l.latitude))))
                            AS  distance, [Address] FROM[dbo].[Assistants] a
                            INNER JOIN GeoLocations l on a.CurrentGeoLocationId = l.Id
                            where a.IsReserved = 0) ast ORDER BY ast.distance";

            // This Query Returns Nearest  Available ( NOT Reserved) Asssistants    -- IsReserved = 0
            // Nearest Distance Calc based on Latitude and Langitude and taken care by Sql Server Order By ast.distance )

            var nearestAssistants = await _assistantRepository.GetAllFromSql(sqlQuery, CancellationToken.None, sqlParameters);
            return nearestAssistants;    
        }

        public async Task<(Assistant? Assistant, ErrorType? ErrorType)> ReserveAssistant(Customer customer, GeoLocation customerLocation)
        {
            Expression<Func<CustomerAssistant, bool>> customerAssistantExpression = c => c.CustomerId == customer.Id && c.Status == ServiceStatus.InProgress.ToString();
            var inProgressAssistance = _customerAssistantRepository.Entity.FirstOrDefault(customerAssistantExpression);
            if(inProgressAssistance != null) // Customer Shouldn't assist by more than one Assistant at the same time.. There shouldn't be any InProgress Assisstances.
            {
                return (null, ErrorType.ConflictError); 
            }

            var nearestAvailableAssistants = await FindNearestAssistants(customerLocation, 1); // Limit set to 1 to get the nearest and try to reserve one. Can be increased and add a loop to check one by one
            if (nearestAvailableAssistants == null || !nearestAvailableAssistants.Any())
            {
                return (null, ErrorType.NotFoundError);
            }
           
            var newCustomerAssistant = new CustomerAssistant(customer.Id);
            bool isReadyToReserve = newCustomerAssistant.ReserveAssistant(nearestAvailableAssistants.First(), customerLocation.Id);
            if (isReadyToReserve)
            {
                var customerAssistant = await _customerAssistantRepository.CreateAsync(newCustomerAssistant);
                return (customerAssistant.Assistant,null);
            }
            return (null, ErrorType.UnKnownError);

            //foreach (var availableAssistant in nearestAvailableAssistants)
            //{
            //    var customerAssistant = await ReserveAssistant(customer, availableAssistant, customerLocation);
            //    if(customerAssistant != null)
            //    {
            //        break;
            //    }
            //}
        }
        public async Task<(bool IsSuccess, ErrorType? ErrorType)> ReleaseAssistant(Customer customer, Assistant assistant)
        {
            var inProgressCustomerAssistance = _customerAssistantRepository.Queryable()
                                                                           .Include(c=>c.Assistant)
                                                                           .FirstOrDefault(c=>c.CustomerId == customer.Id 
                                                                                               && c.Status == ServiceStatus.InProgress.ToString());

            if (inProgressCustomerAssistance == null) // One Active CustomerAssistant Should be InProgress in-order to Release an Assistant
            {
                return (false,ErrorType.NotFoundError);
            }
            bool isReadyToRelease = inProgressCustomerAssistance.ReleaseAssistant();
            if (isReadyToRelease)
            {
                var customerAssistant = await _customerAssistantRepository.UpdateAsync(inProgressCustomerAssistance);
                return (true,null);
            }
            return (false,ErrorType.UnKnownError);
        }

        public async Task<Assistant> UpdateAssistantLocation(Assistant assistant, GeoLocation assistantLocation)
        {
            assistant.CurrentGeoLocationId = assistantLocation.Id;
            return await _assistantRepository.UpdateAsync(assistant);
        }
    }
}
