using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Common.Options
{
    public class MailJetOptions
    {
        public string SendFromName { get; set; }
        public string SendFromEmail { get; set; }
        public string PublicAPIKey { get; set; }
        public string PrivateAPIKey { get; set; }
    }
}