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

        private UserVM user;
        public UserVM User
        {
            get { return user; }
            set { 
                user = value;
                Email = value.Email;
                Roles.Clear();
                UserRoles.Clear();
                UserRoles = new BindingList<string>(value.Roles.Select(r => r.Value).ToList());
                _ = LoadRoles();
                NotifyOfPropertyChange(() => User);
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; NotifyOfPropertyChange(() => Email); }
        }

        private BindingList<string> userRoles = new BindingList<string>();
        public BindingList<string> UserRoles
        {
            get { return userRoles; }
            set { userRoles = value; NotifyOfPropertyChange(() => UserRoles); }
        }

        private string userRole;
        public string UserRole
        {
            get { return userRole; }
            set { 
                userRole = value;
                NotifyOfPropertyChange(() => UserRole);
                NotifyOfPropertyChange(() => CanRemoveRole);
            }
        }

        private BindingList<string> roles = new BindingList<string>();
        public BindingList<string> Roles
        {
            get { return roles; }
            set { roles = value; NotifyOfPropertyChange(() => Roles); }
        }

        private string role;
        public string Role
        {
            get { return role; }
            set { 
                role = value;
                NotifyOfPropertyChange(() => Role);
                NotifyOfPropertyChange(() => CanAddRole);
            }
        }


        // COMMANDS
        public bool CanAddRole => User != null && !string.IsNullOrEmpty(Role);
        public async Task AddRole()
        {
            await _userService.AddRoleToUser(User.Id, Role);
            UserRoles.Add(Role);

            var rand = new Random();
            Users.First(u => u.Id == User.Id).Roles.Add(rand.Next(1000000).ToString(), Role);
            
            Roles.Remove(Role);
        }

        public bool CanRemoveRole => User != null && !string.IsNullOrEmpty(UserRole);
        public async Task RemoveRole()
        {
            await _userService.RemoveRoleFromUser(User.Id, UserRole);
            Roles.Add(UserRole);

            var u = Users.First(u => u.Id == User.Id);
            var r = u.Roles.First(kvp => kvp.Value == UserRole);
            u.Roles.Remove(r.Key);
            
            UserRoles.Remove(UserRole);
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

                await _windowManager.ShowDialogAsync(_messageViewModel, null, settings);
                await TryCloseAsync();
            }
        }

        async Task LoadUsers() => Users = new BindingList<UserVM>(_mapper.Map<List<UserVM>>(await _userService.GetAllAsync()));

        async Task LoadRoles()
        {
            var roles = await _userService.GetAllRolesAsync();
            foreach (var role in roles)
                if (UserRoles.IndexOf(role.Value) < 0)
                    Roles.Add(role.Value);
        }
    }
}
