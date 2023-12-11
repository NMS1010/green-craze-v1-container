using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Common.Enums
{
    public class USER_ROLE
    {
        public static string ADMIN = "ADMIN";
        public static string USER = "USER";
        public static string STAFF = "STAFF";

        public static List<string> Roles = new()
        {
            ADMIN,
            USER,
            STAFF
        };
    }
}