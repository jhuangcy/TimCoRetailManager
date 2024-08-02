using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimCoRetailManager_WPF.Library.Models;
using TimCoRetailManager_WPF.Library.Services;
using TimCoRetailManager_WPF.Models;

namespace TimCoRetailManager_WPF.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IMapper _mapper;
        private readonly IConfigService _configService;
        private readonly IProductService _productService;
        private readonly ISaleService _saleService;
        //private readonly MessageViewModel _messageViewModel;

        public SalesViewModel(IWindowManager windowManager, IMapper mapper, IConfigService configService, IProductService productService, ISaleService saleService/*, MessageViewModel messageViewModel */)
        {
            _windowManager = windowManager;
            _mapper = mapper;
            _configService = configService;
            _productService = productService;
            _saleService = saleService;
            //_messageViewModel = messageViewModel;
        }


        // PROPERTIES
        // Product model is from the lib and doesnt have events when it changes
        //private BindingList<Product> products;
        //public BindingList<Product> Products
        private BindingList<ProductVM> products;
        public BindingList<ProductVM> Products
        {
            get { return products; }
            set { products = value; NotifyOfPropertyChange(() => Products); }
        }

        //private Product product;
        //public Product Product
        private ProductVM product;
        public ProductVM Product
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

        // CartItem model is from the lib and doesnt have events when it changes
        //private BindingList<CartItem> cart = new BindingList<CartItem>();
        //public BindingList<CartItem> Cart
        private BindingList<CartItemVM> cart = new BindingList<CartItemVM>();
        public BindingList<CartItemVM> Cart
        {
            get { return cart; }
            set { cart = value; NotifyOfPropertyChange(() => Cart); }
        }

        private CartItemVM cartItem;
        public CartItemVM CartItem
        {
            get { return cartItem; }
            set
            {
                cartItem = value;
                NotifyOfPropertyChange(() => CartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }

        // https://stackoverflow.com/questions/428798/map-and-reduce-in-net
        // https://stackoverflow.com/questions/37328681/how-to-convert-int-to-decimal-in-net
        //public string Subtotal => Cart.Aggregate(0m, (acc, i) => acc += i.Product.RetailPrice * i.Qty).ToString("C");
        //public string Tax => Cart.Aggregate(0m, (acc, i) => i.Product.Taxable ? acc += i.Product.RetailPrice * i.Qty * (_configService.GetTax() / 100) : acc += 0).ToString("C");
        public string Subtotal => Cart.Sum(i => i.Product.RetailPrice * i.Qty).ToString("C");
        public string Tax => Cart.Where(i => i.Product.Taxable).Sum(i => i.Product.RetailPrice * i.Qty * (_configService.GetTax() / 100)).ToString("C");

        // https://stackoverflow.com/questions/4953037/problem-parsing-currency-text-to-decimal-type
        public string Total => (decimal.Parse(Subtotal, NumberStyles.Currency) + decimal.Parse(Tax, NumberStyles.Currency)).ToString("C");


        // COMMANDS
        public bool CanAddToCart => Qty > 0 && Product?.Qty >= Qty;
        public void AddToCart() 
        {
            var existing = Cart.FirstOrDefault(i => i.Product == Product);
            if (existing == null)
            {
                //Cart.Add(new CartItem { Product = Product, Qty = Qty });
                Cart.Add(new CartItemVM { Product = Product, Qty = Qty });
            }
            else
            {
                existing.Qty += Qty;
                //Cart.Remove(existing);
                //Cart.Add(existing);
            }

            Product.Qty -= Qty;
            Qty = 1;

            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
        }
        
        public bool CanRemoveFromCart => CartItem != null && CartItem?.Qty > 0;
        public void RemoveFromCart()
        {
            CartItem.Product.Qty += 1;
            if (CartItem.Qty > 1)
                CartItem.Qty -= 1;
            else
                cart.Remove(CartItem);
            
            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
            NotifyOfPropertyChange(() => CanAddToCart);
        }

        public bool CanCheckout => Cart.Any();
        public async Task Checkout()
        {
            var sale = new SaleDTO();
            foreach (var item in Cart)
                sale.SaleDetails.Add(new SaleDetailDTO { ProductId = item.Product.Id, Qty = item.Qty });

            await _saleService.PostAsync(sale);
            await ResetView();
        }


        // PRIVATE
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            //Products = new BindingList<Product>(await _productService.GetAllAsync());
            //Products = new BindingList<ProductVM>(_mapper.Map<List<ProductVM>>(await _productService.GetAllAsync()));

            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                // Use IoC to get new instance everytime instead of constructor injection
                var _messageViewModel = IoC.Get<MessageViewModel>();

                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "Message Box";

                if (ex.Message == "Unauthorized")
                    _messageViewModel.Add("Unauthorized", "You do not have permission to access this resource.");
                else
                    _messageViewModel.Add("Error", ex.Message);

                await _windowManager.ShowDialogAsync(_messageViewModel, null, settings);
                await TryCloseAsync();
            }
        }

        async Task LoadProducts() => Products = new BindingList<ProductVM>(_mapper.Map<List<ProductVM>>(await _productService.GetAllAsync()));

        async Task ResetView()
        {
            Cart = new BindingList<CartItemVM>();
            await LoadProducts();

            NotifyOfPropertyChange(() => Subtotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
        }
    }
}
