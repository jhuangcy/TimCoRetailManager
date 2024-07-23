﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_WPF.Library.Models;
using TimCoRetailManager_WPF.Library.Services;

namespace TimCoRetailManager_WPF.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IConfigService _configService;
        private readonly IProductService _productService;

        public SalesViewModel(IConfigService configService, IProductService productService)
        {
            _configService = configService;
            _productService = productService;
        }

        // PROPERTIES
        private BindingList<Product> products;
        public BindingList<Product> Products
        {
            get { return products; }
            set { products = value; NotifyOfPropertyChange(() => Products); }
        }

        private Product product;
        public Product Product
        {
            get { return product; }
            set { 
                product = value; 
                NotifyOfPropertyChange(() => Product);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        private int qty = 1;
        public int Qty
        {
            get { return qty; }
            set { 
                qty = value; 
                NotifyOfPropertyChange(() => Qty);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private BindingList<CartItem> cart = new BindingList<CartItem>();
        public BindingList<CartItem> Cart
        {
            get { return cart; }
            set { cart = value; NotifyOfPropertyChange(() => Cart); }
        }

        // https://stackoverflow.com/questions/428798/map-and-reduce-in-net
        // https://stackoverflow.com/questions/37328681/how-to-convert-int-to-decimal-in-net
        public string Subtotal => Cart.Aggregate(0m, (acc, i) => acc += i.Product.RetailPrice * i.Qty).ToString("C");
        public string Tax => Cart.Aggregate(0m, (acc, i) => i.Product.Taxable ? acc += i.Product.RetailPrice * i.Qty * (_configService.GetTax() / 100) : acc += 0).ToString("C");

        // https://stackoverflow.com/questions/4953037/problem-parsing-currency-text-to-decimal-type
        public string Total => (decimal.Parse(Subtotal, NumberStyles.Currency) + decimal.Parse(Tax, NumberStyles.Currency)).ToString("C");


        // COMMANDS
        public bool CanAddToCart => Qty > 0 && Product?.Qty >= Qty;
        public void AddToCart() 
        {
            var existing = Cart.FirstOrDefault(i => i.Product == Product);
            if (existing == null)
                Cart.Add(new CartItem { Product = Product, Qty = Qty });
            else
            {
                existing.Qty += Qty;
                Cart.Remove(existing);
                Cart.Add(existing);
            }

            Product.Qty -= Qty;
            Qty = 1;

            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }
        
        public bool CanRemoveFromCart => true;
        public void RemoveFromCart()
        {

            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanCheckout => true;
        public void Checkout()
        {

        }


        // LIFECYCLE
        protected override async void OnViewLoaded(object view) => Products = new BindingList<Product>(await _productService.GetAllAsync());
    }
}
