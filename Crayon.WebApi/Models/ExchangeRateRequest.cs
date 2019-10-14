using System;
using System.Collections.Generic;

namespace Crayon.WebApi.Models
{
    public class ExchangeRateRequest
    {
        public DateTime[] DateRange { get; set; }
        public string BaseCurrency { get; set; }
        public string TargetCurrency { get; set; }
    }
}