using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.Library.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Qty { get; set; }

        public string DisplayText => $"{Product.Name} ({Qty})";
    }
}
