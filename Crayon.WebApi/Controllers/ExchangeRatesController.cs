using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crayon.ExternalServices;
using Crayon.ExternalServices.Models;
using Crayon.WebApi.Filters;
using Crayon.WebApi.Models;

namespace Crayon.WebApi.Controllers
{
    public class ExchangeRatesController : ApiController
    {
        private IEcbService EcbService { get; set; }
        public ExchangeRatesController(IEcbService ecbService)
        {
            EcbService = ecbService;
        }

        #region WebApiMethods           

        // GET api/values/5
        [AuthorizeToken, HttpGet]
        [Route("api/ExchangeRates/GetHistory")]
        public ExchangeRateHistoryResponse GetHistory([FromBody] ExchangeRateRequest request)
        {
            if (request == null)
                return new ExchangeRateHistoryResponse
                {
                    Code = 400,
                    ErrorCode = "400",
                    Message = "Error! Invalid Parameters .BaseCurrency ,TargetCurrency  and Dates are required fields."
                };
            if (request.BaseCurrency == null || request.TargetCurrency == null || request.DateRange == null)
                return new ExchangeRateHistoryResponse
                {
                    Code = 400,
                    ErrorCode = "400",
                    Message = "Error! Invalid Parameters .BaseCurrency ,TargetCurrency  and Dates are required fields."
                };

            if (request.DateRange.Length < 2)
                return new ExchangeRateHistoryResponse
                {
                    Code = 400,
                    ErrorCode = "400",
                    Message = "Error! we need atleast 2 dates to get the historical data."
                };
            var dateRange = request.DateRange.OrderBy(x => x).ToList();

            var model = new EcbRequest
            {
                BaseUrl = ConfigurationManager.AppSettings["BaseUrl"],
                SourceCurrency = request.BaseCurrency,
                TargetCurrency = request.TargetCurrency,
                StartDate = dateRange.FirstOrDefault(),
                EndDate = dateRange.LastOrDefault()
            };
            var ecbResponse = EcbService.GetExchangeRateInformation(model);
            var dict = ecbResponse.Rates.Where(p => p.Value.Any(x => x.Key == request.TargetCurrency))
                                                    .ToDictionary(p => p.Key, p => p.Value.FirstOrDefault(x => x.Key == request.TargetCurrency).Value);
            var historyResponse = new ExchangeRateResponse { MinimumRate = GetMinRate(dict), MaximumRate = GetMaxRate(dict), AverageDate = GetAverageRate(ecbResponse.Rates, request.TargetCurrency) };

            return new ExchangeRateHistoryResponse
            {
                Code = 200,
                ErrorCode = "200",
                Message = "Success",
                ExchangeRateResponse = historyResponse
            };
        }

        // POST api/values
        [AuthorizeToken, HttpPost, ApiExplorerSettings(IgnoreApi = true)]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [AuthorizeToken, HttpPut, ApiExplorerSettings(IgnoreApi = true)]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [AuthorizeToken, HttpDelete, ApiExplorerSettings(IgnoreApi = true)]
        public void Delete(int id)
        {
        }

        #endregion

        #region Private


        private ExchangeRate GetMinRate(Dictionary<string, double> result)
        {
            var key = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            var value = result.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;
            return new ExchangeRate { Date = key, Rate = value / 10 };
        }

        private ExchangeRate GetMaxRate(Dictionary<string, double> result)
        {
            var key = result.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            var value = result.Aggregate((l, r) => l.Value > r.Value ? l : r).Value;

            return new ExchangeRate { Date = key, Rate = value / 10 };

        }

        private ExchangeRate GetAverageRate(Dictionary<string, Dictionary<string, double>> result, string targetCurrency)
        {
            var average = result.Values.SelectMany(d => d).GroupBy(kvp => kvp.Key).ToDictionary(g => g.Key, g => g.Average(kvp => kvp.Value)).FirstOrDefault(x => x.Key == targetCurrency);
            var averageKey = result.Values.SelectMany(d => d).GroupBy(kvp => kvp.Key).ToDictionary(g => g.Key, g => g.Average(kvp => kvp.Value));            
            return new ExchangeRate { Rate = average.Value / 10 };
        }

        #endregion
    }
}
