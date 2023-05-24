namespace RoadsideAssistance.Api.Models.Request
{
    public class GeoLocationRequest
    {
        /// <summary>
        ///     The Latitude of the Location.
        /// </summary>
        /// <value>
        ///     The Latitude.
        /// </value>
        /// <example>36.880609</example>
        public double Latitude { get; set; }

        /// <summary>
        ///     The Longitude of the Location.
        /// </summary>
        /// <value>
        ///     The Longitude.
        /// </value>
        /// <example>-77.905518</example>
        public double Longitude { get; set; }
    }
}
