using FluentValidation;
using RoadsideAssistance.Api.Models.Request;

namespace RoadsideAssistance.Api.Validators
{
    public class GeoLocationRequestValidator :AbstractValidator<GeoLocationRequest>
    {
        public GeoLocationRequestValidator() 
        {
            // Virginia State Accceptable Range Latitude (36.5427 to 39.4659)  and Longitude (-83.6753 to -74.9707)
            RuleFor(g => g.Latitude).Must(g => g >= 36.5427 && g <= 39.4659).WithMessage("Gelocation Latitude is Not in Virginia State Latitude range");
            RuleFor(g => g.Longitude).Must(g => g >= -83.6753 && g <= -74.9707).WithMessage("Gelocation Longitude is Not in Virginia State Longitude range");
        }
    }
}
