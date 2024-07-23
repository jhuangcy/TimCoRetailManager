using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.Library.Models
{
    public interface IProduct
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        decimal RetailPrice { get; set; }
        int Qty { get; set; }

        string DisplayText { get; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        public int Qty { get; set; }

        public string DisplayText => $"{Name} ({Qty})";
    }
}
