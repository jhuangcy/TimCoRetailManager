using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_WPF.Library.Models;
using TimCoRetailManager_WPF.Library.Services;

namespace TimCoRetailManager_WPF.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductService _productService;

        public SalesViewModel(IProductService productService)
        {
            _productService = productService;
        }

        // PROPERTIES
        private BindingList<Product> products;
        public BindingList<Product> Products
        {
            get { return products; }
            set { products = value; NotifyOfPropertyChange(() => Products); }
        }

        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; NotifyOfPropertyChange(() => Quantity); }
        }

        private BindingList<string> cart;
        public BindingList<string> Cart
        {
            get { return cart; }
            set { cart = value; NotifyOfPropertyChange(() => Cart); }
        }

        public string Subtotal => "$0.00";
        public string Tax => "$0.00";
        public string Total => "$0.00";


        // COMMANDS
        public bool CanAddToCart => true;
        public void AddToCart()
        {
            
        }

        public bool CanRemoveFromCart => true;
        public void RemoveFromCart()
        {

        }

        public bool CanCheckout => true;
        public void Checkout()
        {

        }


        // LIFECYCLE
        protected override async void OnViewLoaded(object view) => await LoadProducts();
        
        async Task LoadProducts() => Products = new BindingList<Product>(await _productService.GetAllAsync());
    }
}
