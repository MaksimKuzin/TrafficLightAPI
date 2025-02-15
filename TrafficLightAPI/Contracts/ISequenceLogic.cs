using TrafficLightAPI.Models;

namespace TrafficLightAPI.Contracts
{
    public interface ISequenceLogic
    {
        public Task<string> CreateSequence();
        public Task<ObservationResponse> AddObservation(ObservationRequest request);
        public Task ClearData();
    }
}
