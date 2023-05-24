using RoadsideAssistance.Api.Data.DomianEntities;

namespace RoadsideAssistance.Api.Models.Response
{
    public class AssistantResponse 
    {
        public AssistantResponse(Assistant assistant)
        {
            Id = assistant.Id;
            Description = assistant.Description;
            LocationId = assistant.CurrentGeoLocationId;
        }
        /// <summary>
        /// Identfier for Assistant
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Description of an Assistant 
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Location 
        /// </summary>
        public int LocationId { get; set; }

    }
}
