using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crayon.ExternalServices.Models
{
    public class EcbResponse
    {
        public Dictionary<string, Dictionary<string, double>> Rates { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public string Base { get; set; }
        public DateTimeOffset EndAt { get; set; }

    }

}
