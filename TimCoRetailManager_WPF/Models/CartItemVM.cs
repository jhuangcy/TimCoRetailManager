using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.Models
{
    // This is a duplicate of the model in lib, but can emit events
    public class CartItemVM : INotifyPropertyChanged
    {
        public ProductVM Product { get; set; }

        //public int Qty { get; set; }
        private int qty;
        public int Qty
        {
            get { return qty; }
            set { 
                qty = value; 
                CallPropertyChanged(nameof(Qty));
                CallPropertyChanged(nameof(DisplayText));
            }
        }

        public string DisplayText => $"{Product.Name} ({Qty})";

        public event PropertyChangedEventHandler PropertyChanged;
        public void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
