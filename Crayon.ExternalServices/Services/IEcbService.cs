using Crayon.ExternalServices.Models;

namespace Crayon.ExternalServices
{
    public interface IEcbService
    {
        EcbResponse GetExchangeRateInformation(EcbRequest input);
    }
}
