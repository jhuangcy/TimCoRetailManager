using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_WPF.Services;

namespace TimCoRetailManager_WPF.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IApiService _apiService;

        public LoginViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        // PROPERTIES
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; NotifyOfPropertyChange(() => Email); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; NotifyOfPropertyChange(() => CanLogin); }
        }

        // COMMANDS
        public bool CanLogin => Email?.Length > 0 && Password?.Length > 0;
        public async Task Login()
        {
            try
            {
                var user = await _apiService.GetToken(Email, Password);
            }
            catch (Exception ex)
            {
                
            }
        }



    }
}
