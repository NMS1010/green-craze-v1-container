using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Notification
{
    public class CreateNotificationRequest
    {
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string Anchor { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
    }
}