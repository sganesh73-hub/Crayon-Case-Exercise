using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crayon.ExternalServices.Models
{
    public partial class EcbRequest
    {
        public string BaseUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
    }

}
