using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Common.Options
{
    public class JWTConfigOptions
    {
        public string Issuer { get; set; }
        public string SigningKey { get; set; }
        public int Expired { get; set; }
    }
}