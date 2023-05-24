using FluentValidation;
using RoadsideAssistance.Api.Models.Request;

namespace RoadsideAssistance.Api.Filters
{
    public class GeoLocationRequestFilter : IEndpointFilter
    {
        private readonly IValidator<GeoLocationRequest> _validator;

        public GeoLocationRequestFilter(IValidator<GeoLocationRequest> validator)
        {
            _validator = validator;
        }
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var geoLocationRequest = context.GetArgument<GeoLocationRequest>(1);
            var validationResult = await _validator.ValidateAsync(geoLocationRequest);
            if(!validationResult.IsValid) 
            {
                return Results.BadRequest(validationResult.Errors);
            }
            var result = await next(context);
            return result;
        }
    }
}
