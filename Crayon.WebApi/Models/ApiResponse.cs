namespace Crayon.WebApi.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }


    }

    public class ExchangeRateHistoryResponse : ApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public ExchangeRateResponse ExchangeRateResponse { get; set; }

    }


}