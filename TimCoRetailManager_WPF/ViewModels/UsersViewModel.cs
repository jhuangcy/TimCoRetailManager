using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimCoRetailManager_WPF.Library.Models;
using TimCoRetailManager_WPF.Library.Services;
using TimCoRetailManager_WPF.Models;

namespace TimCoRetailManager_WPF.ViewModels
{
    public class UsersViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersViewModel(IWindowManager windowManager, IMapper mapper, IUserService userService)
        {
            _windowManager = windowManager;
            _mapper = mapper;
            _userService = userService;
        }


        // PROPERTIES
        private BindingList<UserVM> users;
        public BindingList<UserVM> Users
        {
            get { return users; }
            set { users = value; NotifyOfPropertyChange(() => Users); }
        }


        // PRIVATE
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            try
            {
                await LoadUsers();
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

                _windowManager.ShowDialog(_messageViewModel, null, settings);
                TryClose();
            }
        }

        async Task LoadUsers() => Users = new BindingList<UserVM>(_mapper.Map<List<UserVM>>(await _userService.GetAllAsync()));
    }
}
