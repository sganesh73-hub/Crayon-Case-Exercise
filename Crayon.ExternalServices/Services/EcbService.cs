using Crayon.ExternalServices.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crayon.ExternalServices
{
    public class EcbService : IEcbService
    {
        private RestClient GetRestClient()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls11;
            var client = new RestClient();
            return client;
        }

        private RestRequest GetDefaultBetaRequest(RestRequest request)
        {
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/json;");
            request.AddHeader("Accept", "application/json");
            return request;
        }

        public EcbResponse GetExchangeRateInformation(EcbRequest input)
        {
            var client = GetRestClient();
            var startDate = input.StartDate.ToString("yyyy-MM-dd");
            var endDate = input.EndDate.ToString("yyyy-MM-dd");
            var parameters = "start_at=" + startDate + "&end_at=" + endDate + "&symbols=" + input.SourceCurrency + "," + input.TargetCurrency;
            var request = new RestRequest(input.BaseUrl + "/history?" + parameters, Method.GET);
            request = GetDefaultBetaRequest(request);
            var result = client.Execute<EcbResponse>(request);
            return result.Data;
        }

    }
}
