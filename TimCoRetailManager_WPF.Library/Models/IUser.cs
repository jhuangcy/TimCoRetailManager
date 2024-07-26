using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoRetailManager_WPF.Library.Models
{
    // This will be a singleton user for wpf
    public interface IUser
    {
        string IdentityUserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        DateTime CreatedDate { get; set; }

        string Token { get; set; }

        void Logout();
    }

    public class User : IUser
    {
        public string IdentityUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }

        public string Token { get; set; }

        public void Logout()
        {
            IdentityUserId = "";
            FirstName = "";
            LastName = "";
            Email = "";
            CreatedDate = DateTime.MinValue;
            Token = "";
        }
    }
}
