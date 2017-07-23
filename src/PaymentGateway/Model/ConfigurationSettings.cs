using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Model
{
    public class ConfigurationSettings
    {
        public string OnSuccessRedirectTo { get; set; }
        public string OnFailedRedirectTo { get; set; }
        public string OnCancelRedirectTo { get; set; }
        public TimeSpan SessionTimeout { get; set; }
    }
}
