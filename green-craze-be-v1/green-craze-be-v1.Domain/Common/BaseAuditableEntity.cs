using green_craze_be_v1.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Domain.Common
{
    public class BaseAuditableEntity<T> : BaseEntity<T>, IAuditableEntity<T>
    {
        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
    }
}