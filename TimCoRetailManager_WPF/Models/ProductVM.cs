using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.Models
{
    // This is a duplicate of the model in lib, but can emit events
    public class ProductVM : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }

        //public int Qty { get; set; }
        private int qty;
        public int Qty
        {
            get { return qty; }
            set {
                qty = value;
                CallPropertyChanged(nameof(Qty));
            }
        }

        public bool Taxable { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
