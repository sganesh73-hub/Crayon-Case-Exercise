namespace Crayon.WebApi.Models
{
    public class ExchangeRateResponse
    {
        public ExchangeRate MinimumRate { get; set; }
        public ExchangeRate MaximumRate { get; set; }
        public ExchangeRate AverageDate { get; set; }
    }

    public class ExchangeRate
    {
        public string Date { get; set; }
        public double Rate { get; set; }
    }


}