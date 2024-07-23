using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.Library.Models
{
    public interface ICartItem
    {
        Product Product { get; set; }
        int Qty { get; set; }

        string DisplayText { get; }
    }

    public class CartItem : ICartItem
    {
        public Product Product { get; set; }
        public int Qty { get; set; }

        public string DisplayText => $"{Product.Name} ({Qty})";
    }
}
