using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.ViewModels
{
    public class LoginViewModel : Screen
    {
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
        public void Login()
        {

        }



    }
}
