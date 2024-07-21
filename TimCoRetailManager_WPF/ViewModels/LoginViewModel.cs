using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_WPF.Library.Services;

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

        private string errorMsg;
        public string ErrorMsg
        {
            get { return errorMsg; }
            set { 
                errorMsg = value; 
                NotifyOfPropertyChange(() => ErrorMsg);
                NotifyOfPropertyChange(() => ErrorVisible);
            }
        }

        public bool ErrorVisible => ErrorMsg?.Length > 0 ? true : false;


        // COMMANDS
        public bool CanLogin => Email?.Length > 0 && Password?.Length > 0;
        public async Task Login()
        {
            ErrorMsg = "";

            try
            {
                var token = await _apiService.GetTokenAsync(Email, Password);
                await _apiService.GetUserAsync(token.access_token);
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
        }
    }
}
