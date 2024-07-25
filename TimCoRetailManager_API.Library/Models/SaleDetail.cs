using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_API.Library.Models
{
    public class SaleDetail
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Tax { get; set; }
    }
}
