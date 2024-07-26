using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_API.Library.Models
{
    public class Inventory
    {
        public int ProductId { get; set; }
        public decimal Cost { get; set; }
        public int Qty { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
