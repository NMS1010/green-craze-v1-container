using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.Inventory
{
    public class ImportProductRequest
    {
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public long ActualInventory {  get; set; }
        public string Note { get; set; }
    }
}
